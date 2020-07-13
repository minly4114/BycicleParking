using ICS.Core.Dtos.Income;
using ICS.Core.Dtos.Outcome;
using ICS.Core.Engine.IProviders;
using ICS.Core.Host.Data.Entities;
using ICS.Core.Host.Engine.IProviders;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ICS.Core.Host.Data;
using ICS.Core.Host.Contracts;
using ICS.Core.Dtos;

namespace ICS.Core.Host.Controllers
{
    [Route("api/v1/cluster")]
    [ApiController]
    //[DisplayName("internal")]
    public class ClusterController : ControllerBase
    {
        private readonly IClustersProvider _clustersProvider;
        private readonly IParkingsProvider _parkingsProvider;
        private readonly IParkingPlacesProvider _parkingPlacesProvider;
        private readonly ILog4netProvider _log4Net;
        private readonly IDbSetProvider<SessionParking> _sessionProvider;
        private readonly IDbSetProvider<ServiceGroup> _serviceProvider;
        private readonly IDbSetProvider<Client> _clientProvider;
        private readonly IDbSetProvider<Parking> _parkingsProviderNew;
        public ClusterController(IClustersProvider clustersProvider,
            IParkingsProvider parkingsProvider, IParkingPlacesProvider parkingPlacesProvider,
            ILog4netProvider log4Net, PostgresContext context,
            IDbSetProvider<SessionParking> sessionProvider, IDbSetProvider<ServiceGroup> serviceProvider,
            IDbSetProvider<Client> clientProvider, IDbSetProvider<Parking> parkingsProviderNew)
        {
            _clustersProvider = clustersProvider;
            _parkingsProvider = parkingsProvider;
            _parkingPlacesProvider = parkingPlacesProvider;
            _log4Net = log4Net;
            _sessionProvider = sessionProvider.Build(context.SessionParkings,context);
            _serviceProvider = serviceProvider.Build(context.ServiceGroups,context);
            _clientProvider = clientProvider.Build(context.Clients,context);
            _parkingsProviderNew = parkingsProviderNew.Build(context.Parkings,context);
        }

        /// <summary>
        /// Регистрирует Кластер
        /// </summary>
        /// <param name="model">Данные о кластере</param>
        /// <returns>Возвращает токен кластера</returns>
        /// <response code="200">Успешно изменено</response>
        /// <response code="201">Успешно создано</response>
        /// <response code="404">Данные кластера не верны</response>
        [HttpPost("register")]
        [ProducesResponseType(typeof(OutcomeClusterToken), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(OutcomeClusterToken), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> RegisterCluster([FromBody] IncomeCluster model)
        {
            if (IPAddress.TryParse(model.IPAddress, out IPAddress address))
            {
                var cluster = _clustersProvider.GetClusters().Include(x => x.Token).FirstOrDefault(x => x.Uuid == model.Uuid);
                if (cluster == null)
                {
                    cluster = new Cluster()
                    {
                        Uuid = model.Uuid,
                        Name = model.Name,
                        IPAddress = address,
                        Port = model.Port
                    };
                    await _clustersProvider.InsertAsync(cluster);
                    var outcomeToken = new OutcomeClusterToken()
                    {
                        Token = cluster.Token.Value,
                        ExpiredAt = cluster.Token.ExpiredAt
                    };
                    _log4Net.Info(typeof(ClusterController).ToString(), "Register cluster", model);
                    return StatusCode((int)HttpStatusCode.Created, outcomeToken);
                }
                else
                {
                    cluster.Name = model.Name;
                    cluster.IPAddress = address;
                    cluster.Port = model.Port;
                    cluster.UpdatedAt = DateTime.UtcNow;
                    await _clustersProvider.UpdateAsync(cluster);
                    var outcomeToken = new OutcomeClusterToken()
                    {
                        Token = cluster.Token.Value,
                        ExpiredAt = cluster.Token.ExpiredAt
                    };
                    _log4Net.Info(typeof(ClusterController).ToString(), "Change cluster", model);
                    return StatusCode((int)HttpStatusCode.OK, outcomeToken);
                }
            }
            _log4Net.Error(typeof(ClusterController).ToString(), HttpStatusCode.BadRequest.ToString(), model);
            return StatusCode((int)HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// Сообщение о работе кластера
        /// </summary>
        /// <param name="token">Токен кластера</param>
        /// <param name="parkingKeepAlives">Параметры работы парковок</param>
        /// <returns>Возвращает токен кластера</returns>
        /// <response code="200">Успешная операция</response>
        /// <response code="400">Неверный формат данных</response>
        /// <response code="403">Токен просрочен или кластер не подтверждён</response>
        [HttpPost("keepAlive")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public async Task<IActionResult> ClusterKeepAlive([FromHeader]Guid token, [FromBody]List<IncomeParkingKeepAlive> parkingKeepAlives)
        {
            var status = _clustersProvider.GetStatusAutorization(token);
            if (status.StatusCode == (int)HttpStatusCode.OK)
            {
                await _clustersProvider.InsertKeepAliveAsync(new ClusterKeepAlive() { Cluster = status.Cluster });
                if (parkingKeepAlives.Count > 0)
                {
                    var parkings = _parkingsProvider.GetParkings().Where(x => x.Cluster == status.Cluster);
                    foreach (var parkingKeepAlive in parkingKeepAlives)
                    {
                        var parking = parkings.FirstOrDefault(x => x.Uuid == parkingKeepAlive.ParkingUuid);
                        await _parkingsProvider.InsertKeepAliveAsync(new ParkingKeepAlive()
                        {
                            Parking = parking,
                            ParkingCondition = parkingKeepAlive.ParkingCondition
                        });
                    }
                }
                _log4Net.Info(typeof(ClusterController).ToString(), "Cluster keepalive" + Environment.NewLine + "Cluster token - " + token.ToString(), parkingKeepAlives);
            }
            else
            {
                _log4Net.Error(typeof(ClusterController).ToString(), ((HttpStatusCode)status.StatusCode).ToString() + Environment.NewLine + "Cluster token - " + token.ToString(), parkingKeepAlives);
            }
            return StatusCode(status.StatusCode);
        }

        /// <summary>
        /// Регистрация парковки в системе
        /// </summary>
        /// <param name="token">Токен кластера</param>
        /// <param name="incomeParking">Информация о парковке</param>
        /// <returns>Возвращает информацию о парковке и парковочных местах</returns>
        /// <response code="202">Успешная операция</response>
        /// <response code="400">Неверный формат данных</response>
        /// <response code="403">Токен просрочен или кластер не подтверждён</response>
        [HttpPost("register/parking")]
        [ProducesResponseType(typeof(OutcomeParking), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public IActionResult RegisterParking([FromHeader]Guid token, IncomeParking incomeParking)
        {
            var status = _clustersProvider.GetStatusAutorization(token);
            if (status.StatusCode == (int)HttpStatusCode.OK)
            {
                var parking = new Parking()
                {
                    Uuid = incomeParking.Uuid,
                    Name = incomeParking.Name,
                    LocationLng = incomeParking.LocationLng,
                    LocationLat = incomeParking.LocationLat,
                    Address = incomeParking.Address,
                    Cluster = status.Cluster,
                    ParkingKeepAlives = new List<ParkingKeepAlive>()
                    {
                        new ParkingKeepAlive()
                        {
                            ParkingCondition=ParkingCondition.Idle,
                        }
                    },
                    ParkingPlaces = incomeParking.ParkingPlaces?.ConvertAll(x => new ParkingPlace()
                    {
                        Uuid = x.Uuid,
                        Level = x.Level,
                        Serial = x.Serial,
                        ParkingPlaceKeepAlives = new List<ParkingPlaceKeepAlive>()
                        {
                            new ParkingPlaceKeepAlive()
                            {
                                ParkingCondition=ParkingPlaceCondition.Free,
                            }
                        }
                    }),
                    ParkingConfigurations = new List<ParkingConfiguration>()
                    {
                        new ParkingConfiguration()
                        {
                            ReservationAllowed=ReservationAllowed.Allowed,
                            MaxNumberDay = 2,
                            MaxNumberPlaces = 5
                        }
                    }
                };
                Parking resultParking;
                try
                {
                    resultParking = _parkingsProviderNew.InsertWithReturnAsync(parking).GetAwaiter().GetResult();
                }
                catch
                {
                    var message = "Данные не соответствуют уникальности";
                    _log4Net.Error(typeof(ClusterController).ToString(), $"Register parking{ Environment.NewLine}Status code {HttpStatusCode.BadRequest} Message {message}{Environment.NewLine}Cluster token : {token}", incomeParking);
                    return StatusCode((int)HttpStatusCode.BadRequest, message);
                }
                var outcomeParking = resultParking.ToOutcome();
                _log4Net.Info(typeof(ClusterController).ToString(), $"Register parking{ Environment.NewLine} Cluster token : {token.ToString()}", outcomeParking);
                return StatusCode((int)HttpStatusCode.Created, outcomeParking);
            }
            else
            {
                _log4Net.Error(typeof(ClusterController).ToString(), $"Register parking{ Environment.NewLine} {(HttpStatusCode)status.StatusCode}{Environment.NewLine}Cluster token : {token}", incomeParking);
            }
            return StatusCode(status.StatusCode);
        }

        /// <summary>
        /// Получение парковок кластера
        /// </summary>
        /// <param name="token">Токен кластера</param>
        /// <returns>Возвращает токен кластера</returns>
        /// <response code="200">Успешная операция</response>
        /// <response code="400">Неверный формат данных</response>
        /// <response code="403">Токен просрочен или кластер не подтверждён</response>
        [HttpGet("getParkings")]
        [ProducesResponseType(typeof(List<OutcomeParking>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public IActionResult GetParkings([FromHeader]Guid token)
        {
            var status = _clustersProvider.GetStatusAutorization(token);
            if (status.StatusCode == (int)HttpStatusCode.OK)
            {
                var outcomeParkings = _parkingsProviderNew.GetQueryable().Include(x=>x.ParkingPlaces).ThenInclude(x=>x.ParkingPlaceKeepAlives).Include(x=>x.ParkingKeepAlives).Where(x=>x.Cluster== status.Cluster).Select(x=>x.ToOutcome()).ToList();
                _log4Net.Info(typeof(ClusterController).ToString(), $"Get parking{Environment.NewLine}Cluster token : { token.ToString()}", outcomeParkings);
                return Ok(outcomeParkings);
            }
            _log4Net.Error(typeof(ClusterController).ToString(), $"{((HttpStatusCode)status.StatusCode).ToString()}Cluster token : {token.ToString()}");
            return StatusCode(status.StatusCode);
        }

        /// <summary>
        /// Получение свободных парковочных мест парковки
        /// </summary>
        /// <param name="token">Токен кластера</param>
        /// <param name="parkingUuid">Uuid парковки, для которой запрашиваются данные</param>
        /// <returns>Возвращает парковочные места</returns>
        /// <response code="200">Успешная операция</response>
        /// <response code="400">Неверный формат данных</response>
        /// <response code="403">Токен просрочен или кластер не подтверждён</response>
        [HttpGet("getFreeParkingPlaces")]
        [ProducesResponseType(typeof(List<OutcomeParkingPlace>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public IActionResult GetFreeParkingPlace([FromHeader]Guid token, [FromHeader]Guid parkingUuid)
        {
            _log4Net.Info(typeof(ClusterController).ToString(), $"Get free parking{Environment.NewLine}Cluster token {token.ToString()}{Environment.NewLine}Parking uuid {parkingUuid.ToString()}");
            var status = _clustersProvider.GetStatusAutorization(token);
            if (status.StatusCode == (int)HttpStatusCode.OK)
            {
                var cluster = status.Cluster;
                var parking = _parkingsProvider.GetParkings().Include(x => x.ParkingPlaces).ThenInclude(x => x.ParkingPlaceKeepAlives).FirstOrDefault(x=>x.Uuid==parkingUuid);
                if (parking == null) return BadRequest();
                var allParkingPlace = parking.ParkingPlaces;
                List<ParkingPlace> freeParkingPaces = new List<ParkingPlace>();
                foreach (var place in allParkingPlace)
                {
                    var lastKeepAlive = place.LastParkingPlaceKeepAlive();
                    if (place.ParkingPlaceKeepAlives==null || place.ParkingPlaceKeepAlives.Count==0|| lastKeepAlive.ParkingCondition == Dtos.ParkingPlaceCondition.Free) freeParkingPaces.Add(place);
                }
                var outcomeParkingPlaces = freeParkingPaces.ConvertAll(x => x.ToOutcome());
                _log4Net.Info(typeof(ClusterController).ToString(), $"Free parking place", outcomeParkingPlaces);
                return Ok(outcomeParkingPlaces);
            }
            _log4Net.Error(typeof(ClusterController).ToString(), $"Get free parking{Environment.NewLine}Cluster token {token.ToString()}{Environment.NewLine}Parking uuid {parkingUuid.ToString()}{Environment.NewLine}Status code {((HttpStatusCode)status.StatusCode).ToString()}");
            return StatusCode(status.StatusCode);
        }

        /// <summary>
        /// Изменение состояния парковочного места на занятое или свободное
        /// </summary>
        /// <param name="token">Токен кластера</param>
        /// <param name="uuidServiceGroup">Uuid сервисной группы, которая паркует велосипед</param>
        /// <param name="incomeParkingPlaceKeepAlive">Информация о состоянии парковочного места</param>
        /// <param name="uuidSession">Id сессии если изменяем парковочние место на свободное, может быть null</param>
        /// <returns>Возвращает токен кластера</returns>
        /// <response code="201">Успешная операция создания сессии</response>
        /// <response code="202">Успешная операция выдачи велосипеда</response>
        /// <response code="400">Неверный формат данных</response>
        /// <response code="403">Токен просрочен или кластер не подтверждён или парковочное место не принадлежит парковке, относящейся к данному кластеру</response>
        [HttpPost("changeParkingPlaceCondition")]
        [ProducesResponseType(typeof(Guid),(int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(Guid), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public IActionResult ChangeParkingPlaceCondition([FromHeader]Guid token, [FromHeader] Guid uuidServiceGroup, [FromHeader] Guid uuidSession, [FromBody] IncomeParkingPlaceKeepAlive incomeParkingPlaceKeepAlive)
        {
            _log4Net.Info(typeof(ClusterController).ToString(), $"ChangeParkingPlaceCondition {Environment.NewLine}Cluster token - {token.ToString()}{Environment.NewLine}Uuid service group{uuidServiceGroup}{Environment.NewLine}Id session {uuidSession}{Environment.NewLine}Parking place keepalive", incomeParkingPlaceKeepAlive);

            var status = _clustersProvider.GetStatusAutorization(token);
            if (status.StatusCode == (int)HttpStatusCode.OK)
            {
                var statusExecution = ChangeParkingPlaceConditionPrivate(status, uuidServiceGroup, incomeParkingPlaceKeepAlive, uuidSession);
                if ((int)statusExecution.StatusCode == 201 || (int)statusExecution.StatusCode == 202)
                {
                    _log4Net.Info(typeof(ClusterController).ToString(), $"ChangeParkingPlaceCondition{Environment.NewLine}Status code: {status.StatusCode}{Environment.NewLine}Message: {statusExecution.Message}{Environment.NewLine}Cluster token - {token.ToString()}{Environment.NewLine}Uuid service group{uuidServiceGroup}{Environment.NewLine}Id session {uuidSession}{Environment.NewLine}Parking place keepalive", incomeParkingPlaceKeepAlive);
                    return StatusCode((int)statusExecution.StatusCode, (Guid)statusExecution.Result);
                }
                else
                {
                    _log4Net.Error(typeof(ClusterController).ToString(), $"ChangeParkingPlaceCondition{Environment.NewLine}Status code: {status.StatusCode}{Environment.NewLine}Message: {statusExecution.Message}{Environment.NewLine}Cluster token - {token.ToString()}{Environment.NewLine}Uuid service group{uuidServiceGroup}{Environment.NewLine}Id session {uuidSession}{Environment.NewLine}Parking place keepalive", incomeParkingPlaceKeepAlive);
                    return StatusCode((int)statusExecution.StatusCode, statusExecution.Message);
                }
            }
            else
            {
                _log4Net.Error(typeof(ClusterController).ToString(), $"ChangeParkingPlaceCondition {Environment.NewLine}Error Code {status.StatusCode}{Environment.NewLine}Cluster token - {token.ToString()}{Environment.NewLine}Uuid service group{uuidServiceGroup}{Environment.NewLine}Id session {uuidSession}{Environment.NewLine}Parking place keepalive", incomeParkingPlaceKeepAlive);
                return StatusCode(status.StatusCode);
            }
        }

        private StatusExecution ChangeParkingPlaceConditionPrivate(StatusAutorization status, Guid uuidServiceGroup, IncomeParkingPlaceKeepAlive incomeParkingPlaceKeepAlive, Guid uuidSession)
        {
            if (incomeParkingPlaceKeepAlive.ParkingCondition == Dtos.ParkingPlaceCondition.Broken) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "This endpoint is not intended to transmit evidence that the parking space is broken." };
            var serviceGroup = _serviceProvider.GetQueryable().FirstOrDefault(x => x.Uuid == uuidServiceGroup && x.Condition != ServiceGroupCondition.Delete);
            if (serviceGroup == null) return new StatusExecution() { StatusCode= HttpStatusCode.BadRequest,Message = "Service group not found" };
            var parkingPlace = _parkingPlacesProvider.GetParkingPlaces().Include(x => x.ParkingPlaceKeepAlives).Include(x => x.Parking).ThenInclude(x => x.Cluster).FirstOrDefault(x => x.Uuid == incomeParkingPlaceKeepAlive.ParkingPlaceUuid);
            if (parkingPlace == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Parking place not found" };
            var lastParkingPlaceKeepAlive = parkingPlace.LastParkingPlaceKeepAlive();
            if (lastParkingPlaceKeepAlive == null) lastParkingPlaceKeepAlive = new ParkingPlaceKeepAlive()
            {
                ParkingCondition = ParkingPlaceCondition.Free
            };
            if (parkingPlace.ParkingPlaceKeepAlives != null && lastParkingPlaceKeepAlive.ParkingCondition == Dtos.ParkingPlaceCondition.Broken) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Parking place is broken" };
            if (!parkingPlace.Parking.Cluster.Equals(status.Cluster)) return new StatusExecution() { StatusCode = HttpStatusCode.Forbidden, Message = "Parking space does not belong to the parking subordinate to this cluster" };

            var keepAlive = new ParkingPlaceKeepAlive()
            {
                ParkingPlace = parkingPlace,
                ParkingCondition = incomeParkingPlaceKeepAlive.ParkingCondition
            };
            if (incomeParkingPlaceKeepAlive.ParkingCondition == Dtos.ParkingPlaceCondition.Free)
            {
                if(lastParkingPlaceKeepAlive.ParkingCondition != ParkingPlaceCondition.Used) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "This place not used" };
                if (uuidSession == null) return  new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Field idSession not be null in this condition" };
                var session = _sessionProvider.GetQueryable().Include(x => x.ParkingPlace).Include(x => x.ServiceGroup).Include(x=>x.SessionChanges).FirstOrDefault(x => x.Uuid == uuidSession);
                if (session == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Session with this id not found" };
                if (session.ParkingPlace != parkingPlace || session.ServiceGroup != serviceGroup || session.GetLastSessionChange().SessionCondition!=SessionCondition.Parked) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Parking place or service group does not fit" };
                keepAlive = _parkingPlacesProvider.InsertKeepAliveWithReturnAsync(keepAlive).Result;
                var sessionChange = new SessionChange()
                {
                    SessionCondition = SessionCondition.Completed,
                    ParkingPlaceKeepAlive = keepAlive
                };
                session.UpdatedAt = DateTime.UtcNow;
                session.SessionChanges.Add(sessionChange);
                _sessionProvider.UpdateAsync(session).Wait();
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Message = "Session changes and parking place condition change succeed",
                    Result = session.Uuid
                };
            }
            else if (incomeParkingPlaceKeepAlive.ParkingCondition == Dtos.ParkingPlaceCondition.Used)
            {
                if(lastParkingPlaceKeepAlive.ParkingCondition != ParkingPlaceCondition.Free) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "This place not free" };
                keepAlive = _parkingPlacesProvider.InsertKeepAliveWithReturnAsync(keepAlive).Result;
                var result = _sessionProvider.InsertWithReturnAsync(new SessionParking()
                {
                    ParkingPlace = parkingPlace,
                    ServiceGroup = serviceGroup,
                    SessionChanges = new List<SessionChange>()
                    {
                        new SessionChange()
                        {
                            SessionCondition = SessionCondition.Parked,
                            ParkingPlaceKeepAlive=keepAlive
                        }
                    }
                }).GetAwaiter().GetResult();
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.Created,
                    Message = "Session created and parking place condition change succeed",
                    Result = result.Uuid
                };
            }
            else
            {
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "For this condition not found action"
                };
            }
        }
        /// <summary>
        /// Получение информации о клиенте
        /// </summary>
        /// <param name="token">Токен кластера</param>
        /// <param name="parkingUuid">Uuid парковки</param>
        /// <param name="rfid">Rfid клиента</param>
        /// <returns>Возвращает данные о клиенте</returns>
        /// <response code="200">Успешная операция</response>
        /// <response code="400">Клиент с таким Rfid не найден</response>
        /// <response code="403">Токен просрочен или кластер не подтверждён</response>
        [HttpGet("getClientInfo")]
        [ProducesResponseType(typeof(OutcomeClient), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public IActionResult GetClientInfo([FromHeader]Guid token, [FromHeader]Guid parkingUuid, [FromHeader]string rfid)
        {
            _log4Net.Info(typeof(ClusterController).ToString(), $"Get client info{Environment.NewLine}Cluster token: {token}");
            var status = _clustersProvider.GetStatusAutorization(token);
            if (status.StatusCode == (int)HttpStatusCode.OK)
            {
                var client = _clientProvider.GetQueryable()
                    .Include(x => x.ServiceGroups).ThenInclude(x => x.ServiceGroup).ThenInclude(x=>x.SessionParkings).ThenInclude(x => x.SessionChanges).ThenInclude(x => x.ParkingPlaceKeepAlive)
                    .Include(x => x.ServiceGroups).ThenInclude(x => x.ServiceGroup).ThenInclude(x => x.SessionParkings).ThenInclude(x=>x.ParkingPlace).FirstOrDefault(x => x.CredentialCard.Rfid == rfid);
                if (client == null)
                {
                    _log4Net.Error(typeof(ClusterController).ToString(), $"Get client info{Environment.NewLine}Cluster token: {token}{Environment.NewLine}Status code{HttpStatusCode.BadRequest.ToString()}");
                    return BadRequest("Client with them rfid not found");
                }
                var outcomeClient = client.ToOutcome(parkingUuid, new SessionCondition[] {SessionCondition.Reservation, SessionCondition.Parked });
                _log4Net.Info(typeof(ClusterController).ToString(), $"Get client info{Environment.NewLine}Cluster token: {token}", outcomeClient);
                return Ok(outcomeClient);
            }
            _log4Net.Error(typeof(ClusterController).ToString(), $"Get free parking{Environment.NewLine}Cluster token: {token.ToString()}{Environment.NewLine}Status code {((HttpStatusCode)status.StatusCode).ToString()}");
            return StatusCode(status.StatusCode);
        }

        /// <summary>
        /// Оповещение о неуспешной операции сессии
        /// </summary>
        /// <param name="token">Токен кластера</param>
        /// <param name="uuidSession">Id сессии если изменяем парковочние место на свободное, может быть null</param>
        /// <param name="sessionCondition">Состояние сессии</param>
        /// <param name="placeCondition">Состояние парковочного места</param>
        /// <param name="parkingCondition">Состояние парковки</param>
        /// <returns>Возвращает токен кластера</returns>
        /// <response code="201">Успешная операция создания сессии</response>
        /// <response code="202">Успешная операция выдачи велосипеда</response>
        /// <response code="400">Неверный формат данных</response>
        /// <response code="403">Токен просрочен или кластер не подтверждён или парковочное место не принадлежит парковке, относящейся к данному кластеру</response>
        [HttpPost("errorParking")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        public IActionResult ChangeErrorParkingPlaceCondition([FromHeader]Guid token, [FromHeader]Guid uuidSession, [FromHeader]SessionCondition sessionCondition, [FromHeader]ParkingPlaceCondition placeCondition, [FromHeader]ParkingCondition parkingCondition)
        {
            _log4Net.Info(typeof(ClusterController).ToString()
                , $"ChangeErrorParkingPlaceCondition{Environment.NewLine}Cluster token - {token}{Environment.NewLine}Uuid session {uuidSession}{Environment.NewLine}Session condition{sessionCondition}{Environment.NewLine}Parking place condition{placeCondition}");

            var status = _clustersProvider.GetStatusAutorization(token);
            if (status.StatusCode == (int)HttpStatusCode.OK)
            {
                var statusExecution = ChangeSessionErrorConditionPrivate(status, uuidSession, sessionCondition, placeCondition, parkingCondition);
                if ((int)statusExecution.StatusCode == 201 || (int)statusExecution.StatusCode == 202)
                {
                    _log4Net.Info(typeof(ClusterController).ToString()
                        , $"ChangeErrorParkingPlaceCondition{Environment.NewLine}Status code: {status.StatusCode}{Environment.NewLine}Message: {statusExecution.Message}{Environment.NewLine}Cluster token - {token}{Environment.NewLine}{Environment.NewLine}Uuid session {uuidSession}{Environment.NewLine}Session condition{sessionCondition}{Environment.NewLine}Parking place condition{placeCondition}");
                }
                else
                {
                    _log4Net.Error(typeof(ClusterController).ToString()
                        , $"ChangeErrorParkingPlaceCondition{Environment.NewLine}Status code: {status.StatusCode}{Environment.NewLine}Message: {statusExecution.Message}{Environment.NewLine}Cluster token - {token}{Environment.NewLine}Uuid session {uuidSession}{Environment.NewLine}Session condition{sessionCondition}{Environment.NewLine}Parking place condition{placeCondition}");
                }
                return StatusCode((int)statusExecution.StatusCode, statusExecution.Message);
            }
            else
            {
                _log4Net.Error(typeof(ClusterController).ToString()
                    , $"ChangeErrorParkingPlaceCondition{Environment.NewLine}Error Code {status.StatusCode}{Environment.NewLine}Cluster token - {token.ToString()}{Environment.NewLine}Uuid session {uuidSession}{Environment.NewLine}Session condition{sessionCondition}{Environment.NewLine}Parking place condition{placeCondition}");
                return StatusCode(status.StatusCode);
            }
        }

        private StatusExecution ChangeSessionErrorConditionPrivate(StatusAutorization status, Guid uuidSession, SessionCondition sessionCondition, ParkingPlaceCondition placeCondition, ParkingCondition parkingCondition)
        {
            var session = _sessionProvider.GetQueryable().Include(x=>x.SessionChanges).Include(x => x.ParkingPlace).ThenInclude(x => x.Parking).FirstOrDefault(x => x.Uuid == uuidSession);
            var parking = status.Cluster.Parkings.FirstOrDefault(x => x.Uuid == session.ParkingPlace.Parking.Uuid);
            
            if ( parking == null) return new StatusExecution { StatusCode = HttpStatusCode.BadRequest, Message = "Parking place do not belong this cluster"};
            var parkingWithkeepAlive = _parkingsProviderNew.GetQueryable().Include(x => x.ParkingKeepAlives).FirstOrDefault(x=>x==parking);
            var lastSessionChange = session.GetLastSessionChange();
            if (lastSessionChange.SessionCondition == SessionCondition.Parked&&sessionCondition==SessionCondition.ErrorParking || lastSessionChange.SessionCondition == SessionCondition.Completed && sessionCondition == SessionCondition.ReturnError)
            {
                session.SessionChanges.Add(new SessionChange()
                {
                    SessionCondition = sessionCondition,
                    ParkingPlaceKeepAlive = new ParkingPlaceKeepAlive()
                    {
                        ParkingPlace = session.ParkingPlace,
                        ParkingCondition = placeCondition
                    }
                });
                session.UpdatedAt = DateTime.UtcNow;
                _sessionProvider.UpdateAsync(session).Wait();
                parkingWithkeepAlive.ParkingKeepAlives.Add(new ParkingKeepAlive()
                {
                    ParkingCondition = parkingCondition,
                });
                _parkingsProvider.UpdateAsync(parkingWithkeepAlive).Wait();
                return new StatusExecution { StatusCode = HttpStatusCode.Accepted, Message = "Change session condition accepted" };
            }
            return new StatusExecution { StatusCode = HttpStatusCode.BadRequest, Message = "This session condition do not change" };
        }
    }
}