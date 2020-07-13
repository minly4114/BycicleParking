using ICS.Core.Dtos.Income;
using ICS.Core.Dtos.Outcome;
using ICS.Core.Engine.IProviders;
using ICS.Core.Host.Contracts;
using ICS.Core.Host.Data;
using ICS.Core.Host.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using ICS.Core.Dtos;

namespace ICS.Core.Host.Controllers
{
    [Route("api/v1/supervisor")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly ILog4netProvider _log4Net;
        private readonly IDbSetProvider<Cluster> _clustersProvider;
        private readonly IDbSetProvider<Worker> _supervisorsProvider;
        private readonly IDbSetProvider<Parking> _parkingsProvider;
        private readonly IDbSetProvider<ParkingPlace> _parkingPlacesProvider;
        private readonly IDbSetProvider<ParkingPlaceKeepAlive> _parkingPlaceKeepAlivesProvider;
        private readonly IDbSetProvider<CredentialCard> _cardProvider;


        public WorkerController(IDbSetProvider<ParkingPlace> parkingPlacesProvider, ILog4netProvider log4Net,
            PostgresContext context, IDbSetProvider<Cluster> clustersProvider,
            IDbSetProvider<Worker> supervisorsProvider, IDbSetProvider<Parking> parkingsProvider,
            IDbSetProvider<ParkingPlaceKeepAlive> parkingPlaceKeepAlivesProvider, IDbSetProvider<CredentialCard> cardProvider)
        {
            _log4Net = log4Net;
            _clustersProvider = clustersProvider.Build(context.Clusters, context);
            _supervisorsProvider = supervisorsProvider.Build(context.Workers, context);
            _parkingsProvider = parkingsProvider.Build(context.Parkings, context);
            _parkingPlacesProvider = parkingPlacesProvider.Build(context.ParkingPlaces, context);
            _parkingPlaceKeepAlivesProvider = parkingPlaceKeepAlivesProvider.Build(context.ParkingPlaceKeepAlives, context);
            _cardProvider = cardProvider.Build(context.CredentialCards, context);
        }

        /// <summary>
        /// Изменяет подтверждение кластера
        /// </summary>
        /// <param name="confirm">Подтверждение</param>
        /// <param name="uuidCluster">Uuid кластера</param>
        /// <returns>Возвращает токен кластера</returns>
        /// <response code="200">Успешно изменено</response>
        /// <response code="400">Кластер не найден</response>
        [HttpPost("confirm/cluster")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ConfirmCluster([FromQuery]bool confirm, [FromHeader] Guid uuidCluster)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), "Request сonfirm cluster" + confirm.ToString(), uuidCluster);
            var cluster = _clustersProvider.GetQueryable().FirstOrDefault(x => x.Uuid == uuidCluster);
            if (cluster == null)
            {
                _log4Net.Error(typeof(WorkerController).ToString(), "Request сonfirm cluster " + confirm.ToString(), HttpStatusCode.BadRequest.ToString());
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            cluster.IsConfirmed = confirm;
            await _clustersProvider.UpdateAsync(cluster);

            _log4Net.Info(typeof(WorkerController).ToString(), "Confirm cluster " + confirm.ToString() + Environment.NewLine + "Success", uuidCluster);
            return StatusCode((int)HttpStatusCode.OK);
        }

        /// <summary>
        /// Получает кластеры
        /// </summary>
        /// <param name="supervisorUuid">Фильтрация по супервизору</param>
        /// <param name="clusterUuid">Фильтрация по кластеру</param>
        /// <returns>Возвращает кластера</returns>
        /// <response code="200">Успешно получено</response>
        [HttpGet("clusters")]
        [ProducesResponseType(typeof(OutcomeCluster), (int)HttpStatusCode.OK)]
        public ActionResult GetClusters([FromHeader]string supervisorUuid, [FromHeader]string clusterUuid)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get all clusters{Environment.NewLine}Supervisor uuid {supervisorUuid}Cluster uuid {clusterUuid}");
            IQueryable<Cluster> clusters = _clustersProvider.GetQueryable();
            if (supervisorUuid != null && supervisorUuid.Length != 0) clusters = clusters.Where(x => x.Supervisor.Uuid == new Guid(supervisorUuid));
            if (clusterUuid != null && clusterUuid.Length != 0) clusters = clusters.Where(x => x.Uuid == new Guid(clusterUuid));
            var outcome = clusters.Include(x => x.KeepAlives).Include(x=>x.Supervisor).Include(x=>x.Parkings).Select(x => x.ToOutcome()).ToList();
            _log4Net.Info(typeof(WorkerController).ToString(), "Get all clusters", outcome);
            return StatusCode((int)HttpStatusCode.OK, outcome);
        }

        /// <summary>
        /// Получает парковки
        /// </summary>
        /// <param name="supervisorUuid">Фильтрация по супервизору</param>
        /// <param name="clusterUuid"> Фильтрация по кластеру</param>
        /// <param name="parkingUuid"> Фильтрация по парковке</param>
        /// <returns>Возвращает парковки</returns>
        /// <response code="200">Успешно получено</response>
        [HttpGet("parkings")]
        [ProducesResponseType(typeof(OutcomeParking), (int)HttpStatusCode.OK)]
        public ActionResult GetParkings([FromHeader]string supervisorUuid, [FromHeader]string clusterUuid, [FromHeader] string parkingUuid)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get parkings{Environment.NewLine}Supervisor uuid {supervisorUuid}Cluster uuid {clusterUuid}");
            IQueryable<Parking> parkings = _parkingsProvider.GetQueryable();
            if(supervisorUuid!=null&&supervisorUuid.Length!=0)
            {
                parkings = parkings.Include(x => x.Cluster).ThenInclude(x => x.Supervisor).Where(x=>x.Cluster.Supervisor.Uuid==new Guid(supervisorUuid));
            }
            else if(clusterUuid!=null&&clusterUuid.Length!=0)
            {
                parkings = parkings.Include(x => x.Cluster).Where(x => x.Cluster.Uuid == new Guid(clusterUuid));
            }
            else if(parkingUuid!=null&&parkingUuid.Length!=0)
            {
                parkings = parkings.Where(x => x.Uuid == new Guid(parkingUuid));
            }
            var outcome = parkings.Select(x => x.ToOutcome()).ToList();
            _log4Net.Info(typeof(WorkerController).ToString(), "Get all parkings", outcome);
            return StatusCode((int)HttpStatusCode.OK, outcome);
        }

        /// <summary>
        /// Получает парковочные места
        /// </summary>
        /// <returns>Возвращает токен кластера</returns>
        /// <response code="200">Успешно получено</response>
        /// <response code="400">Uuid кластера не верен</response>
        [HttpGet("parking/places")]
        [ProducesResponseType(typeof(OutcomeParkingPlace), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult GetParkingPlaces([FromHeader]string supervisorUuid,[FromHeader] string parkingUuid)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get parking places{Environment.NewLine}Supervisor uuid {supervisorUuid}Parking uuid {parkingUuid}");
            IQueryable<ParkingPlace> parkingPlaces = _parkingPlacesProvider.GetQueryable();
            if (supervisorUuid != null && supervisorUuid.Length != 0)
            {
                parkingPlaces = parkingPlaces.Where(x => x.Parking.Cluster.Supervisor.Uuid== new Guid(supervisorUuid));
            }
            else if (parkingUuid != null && parkingUuid.Length != 0)
            {
                parkingPlaces = parkingPlaces.Where(x => x.Parking.Uuid== new Guid(parkingUuid));
            }
            var outcome = parkingPlaces.Select(x => x.ToOutcome()).ToList();
            _log4Net.Info(typeof(WorkerController).ToString(), "Get all parking places", outcome);
            return StatusCode((int)HttpStatusCode.OK, outcome);
        }

        /// <summary>
        /// Получает конфигурацию парковки
        /// </summary>
        /// <returns>Возвращает кнфигурацию парковки</returns>
        /// <response code="200">Успешно получено</response>
        /// <response code="400">Uuid кластера не верен</response>
        [HttpGet("parking/configuration")]
        [ProducesResponseType(typeof(OutcomeParkingConfiguration), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult GetParkingConfiguration([FromHeader] string parkingUuid)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get parking configuration{Environment.NewLine}Parking uuid {parkingUuid}");
            var parking = _parkingsProvider.GetQueryable().Include(x => x.ParkingConfigurations).ThenInclude(x => x.Parking)
                .FirstOrDefault(x => x.Uuid == new Guid(parkingUuid));
            var parkingConfiguration = parking.ParkingConfigurations.OrderByDescending(x=>x.CreatedAt).FirstOrDefault()?.ToOutcome();
            _log4Net.Info(typeof(WorkerController).ToString(), "Get parking configuration", parkingConfiguration);
            return StatusCode((int)HttpStatusCode.OK, parkingConfiguration);
        }

        /// <summary>
        /// Изменяет настройки парковки
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Возвращает статус</returns>
        /// <response code="200">Успешно изменено</response>
        /// <response code="400">Кластер не найден</response>
        [HttpPost("parking/configuration")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult ConfigurationParking(IncomeParkingConfiguration model)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Change parking configuration",model);
            var status = ConfigurationParkingPrivate(model);
            if (status.StatusCode == HttpStatusCode.OK)
            {
                _log4Net.Info(typeof(WorkerController).ToString(), $"Change parking configuration{Environment.NewLine}{status.Message}",model);
            }
            else
            {
                _log4Net.Error(typeof(WorkerController).ToString(), $"Change parking configuration{Environment.NewLine}Status code {status.StatusCode}{Environment.NewLine}{status.Message}", model);
            }
            return StatusCode((int)status.StatusCode,status.Message);
        }
        private StatusExecution ConfigurationParkingPrivate(IncomeParkingConfiguration model)
        {
            var supervisor = _supervisorsProvider.GetQueryable().FirstOrDefault(x => x.Uuid == new Guid( model.UuidSuperviser));
            if (supervisor == null)
            {
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Suervizor not found"
                };
            }
            var parking = _parkingsProvider.GetQueryable().Include(x => x.ParkingConfigurations).FirstOrDefault(x => x.Uuid == new Guid( model.UuidParking));
            if (parking == null)
            {
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Parking not found"
                };
            }
            var config = new ParkingConfiguration()
            {
                MaxNumberDay = model.MaxNumberDay,
                MaxNumberPlaces = model.MaxNumberPlaces,
                ReservationAllowed = model.ReservationAllowed,
                Modifying = supervisor,
            };
            if (parking.ParkingConfigurations != null)
            {
                parking.ParkingConfigurations.Add(config);
                _parkingsProvider.UpdateAsync(parking).Wait();
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success changes"
                };
            }
            else
            {
                parking.ParkingConfigurations = new List<ParkingConfiguration>()
                {
                    config
                };
                _parkingsProvider.UpdateAsync(parking).Wait();
                return new StatusExecution()
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success changes"
                };
            }
        }

        /// <summary>
        ///  Добавляет Supervisor
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Возвращает статус по операции</returns>
        /// <response code="200">Успешно изменено</response>
        /// <response code="400">Кластера не найдены</response>
        [HttpPost("add")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult UpdateSupervisor(IncomeWorker model)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Add supervisor", model);
            var status = UpdateSupervisorPrivate(model);
            if (status.StatusCode == HttpStatusCode.OK)
            {
                _log4Net.Info(typeof(WorkerController).ToString(), $"Add supervisor{Environment.NewLine}{status.Message}", model);
            }
            else
            {
                _log4Net.Error(typeof(WorkerController).ToString(), $"Add supervisor{Environment.NewLine}Status code {status.StatusCode}{Environment.NewLine}{status.Message}", model);
            }
            return StatusCode((int)status.StatusCode, status.Message);
        }
        private StatusExecution UpdateSupervisorPrivate(IncomeWorker model)
        {
            Worker supervisor = _supervisorsProvider.GetQueryable().Include(x => x.ControlledСlusters).FirstOrDefault(x => x.Uuid == new Guid(model.Uuid));
            List<Cluster> clusters = new List<Cluster>();
            if (model.ClusterUuids != null && model.ClusterUuids.Count != 0)
            {
                clusters = _clustersProvider.GetQueryable().Include(x => x.Supervisor).Where(x => model.ClusterUuids.Contains(x.Uuid.ToString())).ToList();
                if (clusters == null || clusters.Count != model.ClusterUuids.Count) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Clusters not found" };
                var clusterOccupied = clusters.FirstOrDefault(x => x.Supervisor != null&&x.Supervisor!=supervisor);
                if (clusterOccupied != null) return new StatusExecution() { StatusCode = HttpStatusCode.Forbidden, Message = "Cluster occupied" };   
            }
            if (supervisor == null)
            {
                supervisor = new Worker()
                {
                    Uuid = new Guid(model.Uuid),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PastName = model.PastName,
                    Role = model.Role,
                };
                if (model.Role.HasFlag(RolePersonnel.Supervisor))
                {
                    supervisor.ControlledСlusters = clusters;
                }
                _supervisorsProvider.InsertAsync(supervisor).Wait();
            }
            else
            {
                supervisor.FirstName = model.FirstName;
                supervisor.LastName = model.LastName;
                supervisor.PastName = model.PastName;
                supervisor.Role = model.Role;
                supervisor.UpdatedAt = DateTime.UtcNow;
                if (model.Role.HasFlag(RolePersonnel.Supervisor) || clusters == null||clusters.Count==0)
                {
                    supervisor.ControlledСlusters = clusters;
                }
                _supervisorsProvider.UpdateAsync(supervisor).Wait();
            }
            try
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.OK, Message = "Succeed added" };
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Database error" };
            }
        }

        /// <summary>
        /// Изменяет подтверждение supervisor
        /// </summary>
        /// <param name="confirm">Подтверждение</param>
        /// <param name="uuidSupervisor">Uuid Supervisor</param>
        /// <returns>Возвращает статус операции</returns>
        /// <response code="200">Успешно изменено</response>
        /// <response code="400">Не верные данные</response>
        [HttpPost("confirm")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult ConfirmSupervisor([FromQuery]bool confirm, [FromHeader] Guid uuidSupervisor)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Request сonfirm supervisor{Environment.NewLine}Supervisor uuid {uuidSupervisor}{Environment.NewLine}Confirm {confirm}");
            var status = ConfirmSupervisorPrivate(confirm,uuidSupervisor);
            if (status.StatusCode == HttpStatusCode.OK)
            {
                _log4Net.Info(typeof(WorkerController).ToString(), $"Request сonfirm supervisor{Environment.NewLine}Supervisor uuid {uuidSupervisor}{Environment.NewLine}Confirm {confirm}{Environment.NewLine}Status code {status.StatusCode}{Environment.NewLine}{status.Message}");
            }
            else
            {
                _log4Net.Error(typeof(WorkerController).ToString(), $"Request сonfirm supervisor{Environment.NewLine}Supervisor uuid {uuidSupervisor}{Environment.NewLine}Confirm {confirm}{Environment.NewLine}Status code {status.StatusCode}{Environment.NewLine}{status.Message}");
            }
            return StatusCode((int)status.StatusCode, status.Message);
        }

        private StatusExecution ConfirmSupervisorPrivate(bool confirm, Guid uuidSupervisor)
        {
            var supervisor = _supervisorsProvider.GetQueryable().FirstOrDefault(x => x.Uuid == uuidSupervisor);
            if (supervisor == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Supervisor not found" };
            supervisor.IsConfirmed = confirm;
            try
            {
                _supervisorsProvider.UpdateAsync(supervisor).Wait();
                return new StatusExecution() { StatusCode = HttpStatusCode.Accepted, Message = "Succeed change" };
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Database error" };
            }
        }

        /// <summary>
        /// Получает данные о supervisor
        /// </summary>
        /// <returns>Возвращает токен кластера</returns>
        /// <response code="200">Успешно получено</response>
        /// <response code="400">Uuid кластера не верен</response>
        [HttpGet("get")]
        [ProducesResponseType(typeof(OutcomeWorker), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult GetSupervisorData([FromHeader]Guid uuidSupervisor,[FromHeader] bool includeClusters)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get supervisor data{Environment.NewLine}Supervisor uuid {uuidSupervisor}");
            var supervisordb = _supervisorsProvider.GetQueryable().Include(x => x.ControlledСlusters).ThenInclude(x=>x.KeepAlives).FirstOrDefault(x => x.Uuid == uuidSupervisor);
            var supervisor = supervisordb.ToOutcome(includeClusters);
            if(supervisor==null)
            {
                _log4Net.Error(typeof(WorkerController).ToString(), $"Get supervosor data{Environment.NewLine}Supervisor uuid {uuidSupervisor}{Environment.NewLine}Status code {HttpStatusCode.BadRequest}");
                return BadRequest();
            }
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get supervisor data{Environment.NewLine}Supervisor uuid {uuidSupervisor}", supervisor);
            return Ok(supervisor);
        }

        /// <summary>
        /// Изменяет информацию о кластере
        /// </summary>
        /// <param name="model">Параметры кластера</param>
        /// <returns>Возвращает статус операции</returns>
        /// <response code="200">Успешно изменено</response>
        /// <response code="400">Не верные данные</response>
        [HttpPost("cluster/update")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult ClusterUpdate(IncomeCluster model)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Update cluster", model);
            var status = ClusterUpdatePrivate(model);
            if (status.StatusCode == HttpStatusCode.OK)
            {
                _log4Net.Info(typeof(WorkerController).ToString(), $"Update cluster{Environment.NewLine}Status code {status.StatusCode}{Environment.NewLine}{status.Message}",model);
            }
            else
            {
                _log4Net.Error(typeof(WorkerController).ToString(), $"Update cluster{Environment.NewLine}Status code {status.StatusCode}{Environment.NewLine}{status.Message}", model);
            }
            return StatusCode((int)status.StatusCode, status.Message);
        }

        private StatusExecution ClusterUpdatePrivate(IncomeCluster model)
        {
            var cluster = _clustersProvider.GetQueryable().FirstOrDefault(x => x.Uuid == model.Uuid);
            if (cluster == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Cluster not found" };
            cluster.Name = model.Name;
            cluster.IPAddress = IPAddress.Parse( model.IPAddress);
            cluster.Port = model.Port;
            try
            {
                _clustersProvider.UpdateAsync(cluster);
                return new StatusExecution() { StatusCode = HttpStatusCode.OK, Message = "Update cluster succeed" };
            }
            catch
            {
                return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Database error" };
            }
            
        }

        /// <summary>
        /// Получает keepAlive парковочных мест
        /// </summary>
        /// <returns>Возвращает все keepalive сортированые от последнего к первому</returns>
        /// <response code="200">Успешно получено</response>
        /// <response code="400">Uuid кластера не верен</response>
        [HttpGet("parking/place/keepAlives")]
        [ProducesResponseType(typeof(List<OutcomeParkingPlaceKeepAlive>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult GetParkingPlaceKeepAlives([FromHeader]string parkingPlaceUuid)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get parking place keep alives{Environment.NewLine}Parking place uuid {parkingPlaceUuid}");
            var keepAlives = _parkingPlaceKeepAlivesProvider.GetQueryable().Where(x => x.ParkingPlace.Uuid == new Guid(parkingPlaceUuid))
                .OrderByDescending(x => x.CreatedAt).Include(x=>x.ParkingPlace).Select(x => x.ToOutcome()).ToList();
            if (keepAlives == null)
            {
                _log4Net.Error(typeof(WorkerController).ToString(), $"Get parking place keep alives{Environment.NewLine}Parking place uuid {parkingPlaceUuid}{Environment.NewLine}Status code {HttpStatusCode.BadRequest}");
                return BadRequest();
            }
            _log4Net.Info(typeof(WorkerController).ToString(), $"Get parking place keep alives{Environment.NewLine}Parking place uuid {parkingPlaceUuid}", keepAlives);
            return Ok(keepAlives);
        }

        /// <summary>
        /// Заводит карты на складе
        /// </summary>
        /// <param name="numberOfCards"> количество необходимых карт</param>
        /// <returns>Возвращает статус операции</returns>
        /// <response code="200">Успешно изменено</response>
        /// <response code="400">Не верные данные</response>
        [HttpPost("credentialCard/add")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult AddCredentialCard(int numberOfCards)
        {
            _log4Net.Info(typeof(WorkerController).ToString(), $"Add credential card number of cards {numberOfCards}");

            var cards = new List<CredentialCard>();
            try
            {
                for (int i = 0; i < numberOfCards; i++)
                {
                    cards.Add(_cardProvider.InsertWithReturnAsync((new CredentialCard()
                    {
                        Condition = CredentialCardCondition.Free
                    })).Result);
                }
                var outcome = cards.ConvertAll(x => x.ToOutcome());
                _log4Net.Info(typeof(WorkerController).ToString(), $"Add credential card number of cards {numberOfCards}{Environment.NewLine}Status code {HttpStatusCode.OK}");
                return Ok(outcome);
            }
            catch
            {
                _log4Net.Error(typeof(WorkerController).ToString(), $"Add credential card number of cards {numberOfCards}{Environment.NewLine}Status code {HttpStatusCode.BadRequest}");
                return BadRequest();
            }
        }
    }
}
