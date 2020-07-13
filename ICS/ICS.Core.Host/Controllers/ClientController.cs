using ICS.Core.Dtos;
using ICS.Core.Dtos.Income;
using ICS.Core.Dtos.Income.Client;
using ICS.Core.Dtos.Outcome;
using ICS.Core.Dtos.OutcomeClient;
using ICS.Core.Engine.IProviders;
using ICS.Core.Host.Contracts;
using ICS.Core.Host.Data;
using ICS.Core.Host.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;

namespace ICS.Core.Host.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly ILog4netProvider _log4Net;
        private readonly IDbSetProvider<Client> _clientsProvider;
        private readonly IDbSetProvider<CredentialCard> _cardProvider;
        private readonly IDbSetProvider<Parking> _parkingsProvider;
        private readonly IDbSetProvider<SessionParking> _sessionsProvider;
        private readonly IDbSetProvider<ServiceGroup> _serviceGroupsProvider;
        public ClientController(ILog4netProvider log4Net, PostgresContext context,
            IDbSetProvider<Client> clientsProvider, IDbSetProvider<CredentialCard> cardProvider,
            IDbSetProvider<Parking> parkingsProvider, IDbSetProvider<SessionParking> sessionsProvider,
            IDbSetProvider<ServiceGroup> serviceGroupsProvider)
        {
            _log4Net = log4Net;
            _clientsProvider = clientsProvider.Build(context.Clients, context);
            _cardProvider = cardProvider.Build(context.CredentialCards, context);
            _parkingsProvider = parkingsProvider.Build(context.Parkings, context);
            _sessionsProvider = sessionsProvider.Build(context.SessionParkings, context);
            _serviceGroupsProvider = serviceGroupsProvider.Build(context.ServiceGroups, context);
        }
        /// <summary>
        /// Регистрирует Клиента
        /// </summary>
        /// <param name="model">Данные о клиенте</param>
        /// <returns>Ничего не возвращает</returns>
        /// <response code="201">Успешно создано</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost("updateClientInfo")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult UpdateClient(IncomeClient model)
        {
            _log4Net.Info(typeof(ClientController).ToString(), "Request for Update client info", model);

            var status = UpdateClientPrivate(model);
            if (status.StatusCode != HttpStatusCode.OK)
            {
                _log4Net.Error(typeof(ClientController).ToString(), $"Update client info{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}", model);
            }
            else
            {
                _log4Net.Info(typeof(ClientController).ToString(), $"Update client info{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}", model);
            }
            return StatusCode((int)status.StatusCode, status.Message);
        }
        private StatusExecution UpdateClientPrivate(IncomeClient model)
        {
            Guid uuid;
            try
            {
                uuid = new Guid(model.ClientUuid);
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Uuid format does not match" };
            }
            var card = _cardProvider.GetQueryable().Include(x=>x.Client).FirstOrDefault(x => x.CardNumber == model.CardNumber);
            if (card == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Такой карты не существует" };
            
            var client = _clientsProvider.GetQueryable().Include(x => x.CredentialCard).FirstOrDefault(x => x.Uuid == uuid);
            if (client == null && card.Condition == CredentialCardCondition.Free)
            {
                card.Condition = CredentialCardCondition.Used;
                client = new Client()
                {
                    FirstName = model.FirstName,
                    PastName = model.PastName,
                    LastName = model.LastName,
                    Phone = model.Phone,
                    Uuid = uuid,
                    ServiceGroups = new List<ClientServiceGroup>()
                    {
                        new ClientServiceGroup
                        {
                            ServiceGroup = new ServiceGroup()
                            {
                                Name = $"{model.FirstName} {model.PastName} {model.LastName}",
                            }
                        }
                    },
                    CredentialCard = card
                };
                _clientsProvider.InsertAsync(client).Wait();
            }
            else
            {
                if (card.Condition == CredentialCardCondition.Used && card.Client != client) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Карта уже используется" };

                card.Condition = CredentialCardCondition.Used;
                client.FirstName = model.FirstName;
                client.PastName = model.PastName;
                client.LastName = model.LastName;
                client.Phone = model.Phone;
                client.CredentialCard = card;

                _clientsProvider.UpdateAsync(client).Wait();
            }
            return new StatusExecution() { StatusCode = HttpStatusCode.OK, Message = "Клиент успешно изменён" };
            
        }

        /// <summary>
        /// Получение информации о клиенте
        /// </summary>
        /// <param name="uuidClient"> uuid клиента</param>
        /// <returns>Возвращает данные о клиенте</returns>
        /// <response code="200">Успешная операция</response>
        /// <response code="400">Клиент с таким uuid не найден</response>
        [HttpGet("getClientInfo")]
        [ProducesResponseType(typeof(OutcomeClient), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public IActionResult GetClientInfo( [FromHeader] string uuidClient)
        {
            _log4Net.Info(typeof(ClusterController).ToString(), $"Get client info{Environment.NewLine}Client uuid: {uuidClient}");
            Guid uuid;
            try
            {
                uuid = new Guid(uuidClient);
            }
            catch
            {
                _log4Net.Error(typeof(ClusterController).ToString(), $"Get client info{Environment.NewLine}Client uuid: {uuidClient}{Environment.NewLine}Status code{HttpStatusCode.BadRequest}");
                return BadRequest();
            }
            
            var client = _clientsProvider.GetQueryable().Include(x=>x.CredentialCard).Include(x => x.ServiceGroups).ThenInclude(x => x.ServiceGroup).ThenInclude(x => x.SessionParkings).FirstOrDefault(x => x.Uuid == uuid);
            if (client == null)
            {
                _log4Net.Error(typeof(ClusterController).ToString(), $"Get client info{Environment.NewLine}Client uuid: {uuidClient}{Environment.NewLine}Status code{HttpStatusCode.BadRequest}");
                return BadRequest("Client with them uuid not found");
            }
            var outcomeClient = client.ToOutcome();
            _log4Net.Info(typeof(ClusterController).ToString(), $"Get client info{Environment.NewLine}Client uuid: {uuidClient}", outcomeClient);
            return Ok(outcomeClient);
        }

        /// <summary>
        /// Получает парковки
        /// </summary>
        /// <param name="parkingUuid"> Фильтрация по парковке</param>
        /// <returns>Возвращает парковки</returns>
        /// <response code="200">Успешно получено</response>
        [HttpGet("parkings")]
        [ProducesResponseType(typeof(OutcomeParking), (int)HttpStatusCode.OK)]
        public ActionResult GetParkings([FromHeader] string parkingUuid)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get parkings{Environment.NewLine}Parking uuid {parkingUuid}");
            IQueryable<Parking> parkings = _parkingsProvider.GetQueryable().Include(x=>x.ParkingPlaces).ThenInclude(x=>x.ParkingPlaceKeepAlives);
            if (parkingUuid != null && parkingUuid.Length != 0)
            {
                parkings = parkings.Where(x => x.Uuid == new Guid(parkingUuid));
            }
            var outcome = parkings.Select(x => x.ToOutcomeClient()).ToList();
            _log4Net.Info(typeof(WorkerController).ToString(), "Get parkings", outcome);
            return StatusCode((int)HttpStatusCode.OK, outcome);
        }

        /// <summary>
        /// Бронирует парковочное место
        /// </summary>
        /// <param name="reservationData"></param>
        /// <returns>Возвращает статус броинирования</returns>
        /// <response code="200">Успешно получено</response>
        /// <response code="400">Данные запроса неверны</response>
        [HttpPost("place/reservation")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult ReservationPlace(IncomeClientReservation reservationData)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Reservation place", reservationData);
            var status = ReservationPlacePrivate(reservationData);
            if (status.StatusCode != HttpStatusCode.OK)
            {
                _log4Net.Error(typeof(ClientController).ToString(), $"Reservation place{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}", reservationData);
            }
            else
            {
                _log4Net.Info(typeof(ClientController).ToString(), $"Reservation place{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}", reservationData);
            }
            return StatusCode((int)status.StatusCode, status.Message);
        }
        private StatusExecution ReservationPlacePrivate(IncomeClientReservation reservationData)
        {
            var serviceGroup = _serviceGroupsProvider.GetQueryable().Include(x=>x.SessionParkings).ThenInclude(x=>x.SessionChanges).FirstOrDefault(x => x.Uuid == new Guid(reservationData.UuidServiceGroup) && x.Condition != ServiceGroupCondition.Delete);
            if (serviceGroup == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Сенрвисная группа не найдена!" };
            if(serviceGroup.SessionParkings.FirstOrDefault(x=>x.GetLastSessionChange().SessionCondition==SessionCondition.Reservation)!= null) return new StatusExecution(){ StatusCode = HttpStatusCode.BadRequest, Message = "Невозможно забронировать больше одного места!"};

            var parking = _parkingsProvider.GetQueryable().Include(x=>x.ParkingConfigurations).Include(x=>x.ParkingPlaces).FirstOrDefault(x => x.Uuid == new Guid(reservationData.UuidParking));
            if (parking == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Такая парковка не существует!" };
            var parkingConfiguration = parking.GetConfiguration();

            reservationData.StartParking = reservationData.StartParking.Date;
            reservationData.EndParking = reservationData.EndParking.Date;

            var dateCheck = DateTime.UtcNow;
            TimeSpan time = new TimeSpan(parkingConfiguration.MaxNumberDay, 0, 0, 0);
            dateCheck += time;
            if (dateCheck < reservationData.EndParking) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Бронинрование на данное время невозможно!" };

            List<DateTime> dateTimes = new List<DateTime>();
            TimeSpan span = new TimeSpan(1, 0, 0, 0);
            dateTimes.Add(reservationData.StartParking);
            var dateTemp = reservationData.StartParking;
            do
            {
                dateTemp += span;
                dateTimes.Add(dateTemp);
            } while (dateTemp <= reservationData.EndParking);

            Dictionary<DateTime, List<SessionParking>> sessionsParking = new Dictionary<DateTime, List<SessionParking>>();
            foreach (var date in dateTimes)
            {
                var sessions = _sessionsProvider.GetQueryable().Include(x => x.ParkingPlace).ThenInclude(x => x.Parking).Include(x => x.ParkingPlace).ThenInclude(x => x.ParkingPlaceKeepAlives).Include(x => x.SessionChanges)
                    .Where(x => x.ParkingPlace.Parking == parking
                    && x.StartParking <= date && x.EndParking >= date
                    ).ToList();
                
                sessions = sessions.Where(x => (x.GetLastSessionChange().SessionCondition == SessionCondition.Reservation || x.GetLastSessionChange().SessionCondition == SessionCondition.Parked) 
                && x.ParkingPlace.LastParkingPlaceKeepAlive().ParkingCondition != ParkingPlaceCondition.Broken).ToList();
                sessionsParking.Add(date, sessions);
            }
            foreach(var sessions in sessionsParking)
            {
                if (sessions.Value.Count > parkingConfiguration.MaxNumberPlaces) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Количество забронированных мест превышает максимальное!" };
            }

            HashSet<ParkingPlace> listBusyPlace = new HashSet<ParkingPlace>();
            foreach(var sessions in sessionsParking)
            {
                sessions.Value.ForEach(x => listBusyPlace.Add(x.ParkingPlace));
            }
            ParkingPlace freePlace = parking.ParkingPlaces.FirstOrDefault();
            if (listBusyPlace.Count != 0)
            {
                freePlace = parking.ParkingPlaces.FirstOrDefault(x => !listBusyPlace.Contains(x));
            }
            if (freePlace == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Свободных парковочных мест на данные даты не существует!" };
            var session = new SessionParking()
            {
                StartParking = reservationData.StartParking,
                EndParking = reservationData.EndParking,
                ParkingPlace = freePlace,
                ServiceGroup = serviceGroup,
                SessionChanges = new List<SessionChange>()
                {
                    new SessionChange()
                    {
                        SessionCondition = SessionCondition.Reservation
                    }
                },
            };
            _sessionsProvider.InsertAsync(session).Wait();
            return new StatusExecution() { StatusCode = HttpStatusCode.OK, Message = "Парковочное место успешно забронировано!" };
        }

        /// <summary>
        /// Получает сервисные группы в коротком формате
        /// </summary>
        /// <param name="uuidClient">Uuid клиента</param>
        /// <returns>Возвращает парковки</returns>
        /// <response code="200">Успешно получено</response>
        [HttpGet("groups/short")]
        [ProducesResponseType(typeof(List<OutcomeClientShortServiceGroup>), (int)HttpStatusCode.OK)]
        public ActionResult GetShortServiceGroups([FromHeader] string uuidClient)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get service groups{Environment.NewLine}Client uuid {uuidClient}");
            var serviceGroups = _serviceGroupsProvider.GetQueryable().Where(x => x.Clients.FirstOrDefault(y => y.ClientUuid == new Guid(uuidClient)) != null && x.Condition != ServiceGroupCondition.Delete).Select(x=> x.ToOutcomeClientShort()).ToList();
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get service groups{Environment.NewLine}Client uuid {uuidClient}", serviceGroups);
            return StatusCode((int)HttpStatusCode.OK, serviceGroups);
        }

        /// <summary>
        /// Получает клиента по номеру карты или номеру телефона
        /// </summary>
        /// <param name="cardNumber">Номер карты</param>
        /// <returns>Возвращает данные о клиенте</returns>
        /// <response code="200">Успешно возвращено</response>
        [HttpGet("short/info")]
        [ProducesResponseType(typeof(OutcomeClientShortInfo), (int)HttpStatusCode.OK)]
        public ActionResult GetOtherClients([FromHeader] string cardNumber)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get client info{Environment.NewLine}Card number {cardNumber}");
            if (cardNumber == null || cardNumber.Length == 0)
            {
                var message = "Номер карты пустой";
                _log4Net.Error(typeof(WorkerController).ToString(), $"Get client info{Environment.NewLine}Card number {cardNumber}{Environment.NewLine}Status code {HttpStatusCode.BadRequest} Message {message}");
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }
            var clients = _clientsProvider.GetQueryable().Where(x => x.CredentialCardNumber == cardNumber);
            var outcome = clients.FirstOrDefault()?.ToOutcomeShort();
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get client info{Environment.NewLine}Card number {cardNumber}", outcome);
            return StatusCode((int)HttpStatusCode.OK, outcome);
        }

        /// <summary>
        /// Создание сервисной группы
        /// </summary>
        /// <param name="groupData"></param>
        /// <returns>Возвращает статус операции</returns>
        /// <response code="200">Успешно получено</response>
        /// <response code="400">Данные запроса неверны</response>
        [HttpPost("servicwGroup/create")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult CreateServiceGroup(IncomeClientServiceGroup groupData)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Create service group", groupData);
            var status = CreateServiceGroupPrivate(groupData);
            if (status.StatusCode != HttpStatusCode.OK)
            {
                _log4Net.Error(typeof(ClientController).ToString(), $"Create service group{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}", groupData);
            }
            else
            {
                _log4Net.Info(typeof(ClientController).ToString(), $"Create service group{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}", groupData);
            }
            return StatusCode((int)status.StatusCode, status.Message);
        }
        private StatusExecution CreateServiceGroupPrivate(IncomeClientServiceGroup groupData)
        {
            Guid uuidCreator;
            try
            {
            uuidCreator = new Guid(groupData.UuidCreator);
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Uuid создателя не является guid" };
            }
            var creator = _clientsProvider.GetQueryable().FirstOrDefault(x => x.Uuid == uuidCreator);
            if(creator == null) return new StatusExecution(){ StatusCode = HttpStatusCode.BadRequest, Message="Client c таким uuid не найден" };
            var clients = _clientsProvider.GetQueryable().Where(x => groupData.ClientCardNumbers.Contains(x.CredentialCardNumber)).Select(x=> new ClientServiceGroup() { Client=x }).ToList();
            if (clients.Count != groupData.ClientCardNumbers.Count) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Клиенты с такими номерами карт не найдены" };
            clients.Add(new ClientServiceGroup() { Client = creator });
            var group = new ServiceGroup()
            {
                Clients = clients,
                Name = groupData.Name,
                UuidCreator = uuidCreator
            };
            try
            {
                _serviceGroupsProvider.InsertAsync(group).Wait();
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Невозможно создать группу в бд" };
            }
            return new StatusExecution() { StatusCode = HttpStatusCode.OK, Message = "Группа успешно созлдана" };
        }

        /// <summary>
        /// Получение списка сервисных групп
        /// </summary>
        /// <param name="uuidClient">Uuid клиента</param>
        /// <returns>Возвращает сервисные группы</returns>
        /// <response code="200">Успешно возвращено</response>
        /// <response code="400">Не верные данные</response>
        [HttpGet("groups")]
        [ProducesResponseType(typeof(List<OutcomeClientServiceGroup>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult GetClientServiceGroup([FromHeader, Required(AllowEmptyStrings = false)] string uuidClient)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get client service group{Environment.NewLine}Uuid client {uuidClient}");
            Guid uuidCreator;
            try
            {
                uuidCreator = new Guid(uuidClient);
            }
            catch
            {
                var message = "Uuid клиента не является guid";
                _log4Net.Error(typeof(WorkerController).ToString(), $"Get client service group{Environment.NewLine}Uuid client {uuidClient}{Environment.NewLine}Status code {HttpStatusCode.BadRequest} Message {message}");
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }
            var serviceGroups = _serviceGroupsProvider.GetQueryable().Include(x=>x.Clients).ThenInclude(x=>x.Client).Where(x => x.Clients.FirstOrDefault(y => y.ClientUuid == new Guid(uuidClient)) != null&&x.Condition!=ServiceGroupCondition.Delete).Select(x=>x.ToClientOutcome(uuidCreator)).ToList();
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get client service group{Environment.NewLine}Uuid client {uuidClient}", serviceGroups);
            return StatusCode((int)HttpStatusCode.OK, serviceGroups);
        }

        /// <summary>
        /// Изменение сервисной группы
        /// </summary>
        /// <param name="uuidServiceGroup">Uuid сервисной группы</param>
        /// <param name="groupData">Информация о сервисной группе</param>
        /// <returns>Возвращает статус операции</returns>
        /// <response code="200">Успешно изменено</response>
        /// <response code="400">Данные запроса неверны</response>
        [HttpPut("group/change")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult ChangeServiceGroup([FromHeader, Required(AllowEmptyStrings = false)] string uuidServiceGroup, IncomeClientServiceGroup groupData)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Change service group", groupData);
            var status = ChangeServiceGroupPrivate(uuidServiceGroup,groupData);
            if (status.StatusCode != HttpStatusCode.OK)
            {
                _log4Net.Error(typeof(ClientController).ToString(), $"Change service group{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}", groupData);
            }
            else
            {
                _log4Net.Info(typeof(ClientController).ToString(), $"Change service group{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}", groupData);
            }
            return StatusCode((int)status.StatusCode, status.Message);
        }

        private StatusExecution ChangeServiceGroupPrivate(string uuidServiceGroup, IncomeClientServiceGroup groupData)
        {
            Guid guidServiceGroup;
            try
            {
                guidServiceGroup = new Guid(uuidServiceGroup);
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Uuid сервисной группы не является guid" };
            }
            Guid uuidCreator;
            try
            {
                uuidCreator = new Guid(groupData.UuidCreator);
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Uuid создателя не является guid" };
            }

            var serviceGroup = _serviceGroupsProvider.GetQueryable().Include(x => x.Clients).ThenInclude(x => x.Client).FirstOrDefault(x => x.Uuid == guidServiceGroup && x.Condition != ServiceGroupCondition.Delete);
            if (serviceGroup == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Сервисная группа не найдена" };

            var creator = _clientsProvider.GetQueryable().FirstOrDefault(x => x.Uuid == uuidCreator);
            if (creator == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Client c таким uuid не найден" };

            if (serviceGroup.UuidCreator != uuidCreator) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Создатель не совпадает" };

            var clients = _clientsProvider.GetQueryable().Where(x => groupData.ClientCardNumbers.Contains(x.CredentialCardNumber)).ToList();
            if (clients.Count != groupData.ClientCardNumbers.Count) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Клиенты с такими номерами карт не найдены" };

            serviceGroup.Name = groupData.Name;
            serviceGroup.Clients = clients.ConvertAll(x => new ClientServiceGroup()
            {
                Client = x
            });
            if(!clients.Contains(creator))serviceGroup.Clients.Add(new ClientServiceGroup() { Client = creator });
            serviceGroup.UpdatedAt = DateTime.UtcNow;

            try
            {
                _serviceGroupsProvider.UpdateAsync(serviceGroup).Wait();
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Невозможно изменить группу в бд" };
            }
            return new StatusExecution() { StatusCode = HttpStatusCode.OK, Message = "Группа успешно изменена" };
        }

        /// <summary>
        /// Удаление сервисной группы
        /// </summary>
        /// <param name="uuidServiceGroup">Uuid сервисной группы</param>
        /// <param name="uuidCreator">Uuid создателя группы</param>
        /// <returns>Возвращает статус операции</returns>
        /// <response code="200">Успешно удалена</response>
        /// <response code="400">Данные запроса неверны</response>
        [HttpDelete("group/delete")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult DeleteServiceGroup([FromHeader, Required(AllowEmptyStrings = false)] string uuidServiceGroup, [FromHeader, Required(AllowEmptyStrings = false)] string uuidCreator)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Delete service group{Environment.NewLine}Uuid service group {uuidServiceGroup} Uuid creator {uuidCreator}");
            var status = DeleteServiceGroupPrivate(uuidServiceGroup, uuidCreator);
            if (status.StatusCode != HttpStatusCode.OK)
            {
                _log4Net.Error(typeof(ClientController).ToString(), $"Delete service group{Environment.NewLine}Uuid service group {uuidServiceGroup} Uuid creator {uuidCreator}{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}");
            }
            else
            {
                _log4Net.Info(typeof(ClientController).ToString(), $"Delete service group{Environment.NewLine}Uuid service group {uuidServiceGroup} Uuid creator {uuidCreator}{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}");
            }
            return StatusCode((int)status.StatusCode, status.Message);
        }

        private StatusExecution DeleteServiceGroupPrivate(string uuidServiceGroup, string uuidCreator)
        {
            Guid guidServiceGroup;
            try
            {
                guidServiceGroup = new Guid(uuidServiceGroup);
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Uuid сервисной группы не является guid" };
            }
            Guid guidCreator;
            try
            {
                guidCreator = new Guid(uuidCreator);
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Uuid создателя не является guid" };
            }

            var serviceGroup = _serviceGroupsProvider.GetQueryable().Include(x => x.Clients).ThenInclude(x => x.Client).Include(x=>x.SessionParkings).ThenInclude(x=>x.SessionChanges)
                .FirstOrDefault(x => x.Uuid == guidServiceGroup 
                &&x.Condition != ServiceGroupCondition.Delete);
            if (serviceGroup == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Сервисная группа не найдена" };

            SessionParking incompleteSession = null;
            if (serviceGroup.SessionParkings != null)
            {
                incompleteSession = serviceGroup.SessionParkings.FirstOrDefault(y => y.GetLastSessionChange().SessionCondition == SessionCondition.Parked || y.GetLastSessionChange().SessionCondition == SessionCondition.Reservation);
            }
            if (incompleteSession != null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Сервисная группа имеет не завершённые сессии" };

            var creator = _clientsProvider.GetQueryable().FirstOrDefault(x => x.Uuid == guidCreator);
            if (creator == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Client c таким uuid не найден" };

            if (serviceGroup.UuidCreator != guidCreator) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Создатель не совпадает" };

            serviceGroup.Condition = ServiceGroupCondition.Delete;
            serviceGroup.UpdatedAt = DateTime.UtcNow;

            try
            {
                _serviceGroupsProvider.UpdateAsync(serviceGroup).Wait();
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Невозможно удалить группу" };
            }
            return new StatusExecution() { StatusCode = HttpStatusCode.OK, Message = "Группа успешно удалена" };
        }


        /// <summary>
        /// Получение списка сессий клиента
        /// </summary>
        /// <param name="uuidClient">Uuid клиента</param>
        /// <returns>Возвращает сессии</returns>
        /// <response code="200">Успешно возвращено</response>
        /// <response code="400">Не верные данные</response>
        [HttpGet("sessions")]
        [ProducesResponseType(typeof(List<OutcomeClientServiceGroup>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult GetClientSession([FromHeader, Required(AllowEmptyStrings = false)] string uuidClient)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get client session {Environment.NewLine}Uuid client {uuidClient}");
            Guid guidClient;
            try
            {
                guidClient = new Guid(uuidClient);
            }
            catch
            {
                var message = "Uuid клиента не является guid";
                _log4Net.Error(typeof(WorkerController).ToString(), $"Get client session {Environment.NewLine}Uuid client {uuidClient}{Environment.NewLine}Status code {HttpStatusCode.BadRequest} Message {message}");
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }
            var session = _sessionsProvider.GetQueryable().Include(x=>x.SessionChanges).Include(x => x.ServiceGroup).ThenInclude(x=>x.Clients).ThenInclude(x=>x.Client).Include(x=>x.ParkingPlace).ThenInclude(x => x.Parking).Where(x => x.ServiceGroup.Clients.FirstOrDefault(x=>x.Client.Uuid == guidClient)!=null).Select(x => x.ToOutcomeClient()).ToList();
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get client session {Environment.NewLine}Uuid client {uuidClient}", session);
            return StatusCode((int)HttpStatusCode.OK, session);
        }

        /// <summary>
        /// Отмена бронирования
        /// </summary>
        /// <param name="uuidSession">Uuid сессии</param>
        /// <param name="uuidClient">Uuid клиента</param>
        /// <returns>Возвращает статус операции</returns>
        /// <response code="200">Успешно изменено</response>
        /// <response code="400">Данные запроса неверны</response>
        [HttpPut("session/cancel")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult SessionCancel([FromHeader, Required(AllowEmptyStrings = false)] string uuidSession, [FromHeader, Required(AllowEmptyStrings = false)] string uuidClient)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Cancel session{Environment.NewLine} Uuid session: {uuidSession}");
            var status = SessionCancelPrivate(uuidSession, uuidClient);
            if (status.StatusCode != HttpStatusCode.OK)
            {
                _log4Net.Error(typeof(ClientController).ToString(), $"Cancel session{Environment.NewLine} Uuid session: {uuidSession}{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}");
            }
            else
            {
                _log4Net.Info(typeof(ClientController).ToString(), $"Cancel session{Environment.NewLine} Uuid session: {uuidSession}{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}");
            }
            return StatusCode((int)status.StatusCode, status.Message);
        }

        private StatusExecution SessionCancelPrivate(string uuidSession, string uuidClient)
        {
            Guid guidSession;
            try
            {
                guidSession = new Guid(uuidSession);
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Uuid сессии не является guid" };
            }
            Guid guidClient;
            try
            {
                guidClient = new Guid(uuidClient);
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Uuid клиента не является guid" };
            }

            var client = _clientsProvider.GetQueryable().FirstOrDefault(x => x.Uuid == guidClient);
            if (client == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Клиент с таким uuid не найден" };

            var session = _sessionsProvider.GetQueryable().Include(x=>x.ServiceGroup).ThenInclude(x=>x.Clients).ThenInclude(x=>x.Client).Include(x=>x.SessionChanges).FirstOrDefault(x => x.Uuid == guidSession);
            if (session == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Сессия с таким uuid не найдена" };
            if (session.ServiceGroup.Clients.FirstOrDefault(x => x.Client.Uuid == guidClient) == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Указанная сессия не является сессией указанного клиента" };
            if (session.GetLastSessionChange().SessionCondition != SessionCondition.Reservation) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Сессия не находится в статусе бронирования" };
            TimeSpan time = new TimeSpan(1, 0, 0);
            if (session.StartParking - DateTime.UtcNow < time) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Время возможности отмены бронирования истекло" };
            session.SessionChanges.Add(new SessionChange
            {
                SessionCondition = SessionCondition.ReservationCanceledUser
            });
            try
            {
                _sessionsProvider.UpdateAsync(session).Wait();
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Невозможно изменить состояние сессии" };
            }
            return new StatusExecution() { StatusCode = HttpStatusCode.OK, Message = "Бронирование успешно отменено" };
        }
    }
}