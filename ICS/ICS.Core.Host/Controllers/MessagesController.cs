using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using ICS.Core.Dtos;
using ICS.Core.Dtos.Income;
using ICS.Core.Dtos.Outcome;
using ICS.Core.Engine.IProviders;
using ICS.Core.HelperEntities;
using ICS.Core.Host.Data;
using ICS.Core.Host.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ICS.Core.Host.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDbSetProvider<Dialog> _dialogsProvider;
        private readonly IDbSetProvider<Message> _messagesProvider;
        private readonly IDbSetProvider<Client> _clientProvider;
        private readonly IDbSetProvider<Worker> _workersProvider;
        private readonly IDbSetProvider<SessionParking> _sessionsProvider;
        private readonly IDbSetProvider<Participant> _participantsProvider;
        private readonly ILog4netProvider _log4Net;
        public MessagesController(PostgresContext context
            , ILog4netProvider log4NetProvider
            , IDbSetProvider<Dialog> dialogsProvider
            , IDbSetProvider<Message> messagesProvider
            , IDbSetProvider<Client> clientsProvider
            , IDbSetProvider<Worker> workersProvider
            , IDbSetProvider<SessionParking> sessionsParking
            , IDbSetProvider<Participant> participantsProvider)
        {
            _log4Net = log4NetProvider;
            _dialogsProvider = dialogsProvider.Build(context.Dialogs, context);
            _messagesProvider = messagesProvider.Build(context.Messages, context);
            _clientProvider = clientsProvider.Build(context.Clients, context);
            _workersProvider = workersProvider.Build(context.Workers, context);
            _sessionsProvider = sessionsParking.Build(context.SessionParkings, context);
            _participantsProvider = participantsProvider.Build(context.Participants, context);
        }

        /// <summary>
        /// Создаёт новый диалог
        /// </summary>
        /// <param name="model">Сообщение</param>
        /// <returns>Ничего не возвращает</returns>
        /// <response code="201">Успешно создано</response>
        /// <response code="500">Ошибка сервера</response>
        [HttpPost("send")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public ActionResult CreateDialog(IncomeMessage model)
        {
            _log4Net.Info(typeof(MessagesController).ToString(), "Message send", model);

            var status = CreateDialogPrivate(model);
            if (status.StatusCode != HttpStatusCode.OK)
            {
                _log4Net.Error(typeof(MessagesController).ToString(), $"Message send{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}", model);
            }
            else
            {
                _log4Net.Info(typeof(MessagesController).ToString(), $"Message send{Environment.NewLine}Status code {status.StatusCode} Message {status.Message}", model);
            }
            return StatusCode((int)status.StatusCode, status.Message);
        }
        private StatusExecution CreateDialogPrivate(IncomeMessage model)
        {
            var session = _sessionsProvider.GetQueryable()
                .Include(x=>x.ServiceGroup)
                .ThenInclude(x=>x.Clients)
                .ThenInclude(x=>x.Client)
                .Include(x=>x.ParkingPlace)
                .ThenInclude(x=>x.Parking)
                .ThenInclude(x=>x.Cluster)
                .ThenInclude(x=>x.Supervisor)
                .FirstOrDefault(x => x.Uuid == model.SessionUuid);
            if(session==null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Сессии с таким uuid не существует!" };
            Dialog dialog = null;
            if(model.DialogUuid!=null)
            {
                dialog = _dialogsProvider.GetQueryable()
                    .Include(x=>x.Messages)
                    .Include(x=>x.Participants)
                    .ThenInclude(x=>x.Participant)
                    .FirstOrDefault(x => x.Uuid == model.DialogUuid);
                if(dialog==null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Диалог с таким uuid не существует!" };
            }
            if (model.Sender.Type== Dtos.TypeUser.Client)
            {
                var client = _clientProvider.GetQueryable().FirstOrDefault(x => x.Uuid == model.Sender.Uuid);
                if (client == null) return new StatusExecution(){ StatusCode = HttpStatusCode.BadRequest, Message = "Клиента с таким uuid не существует!" };
                if(session.ServiceGroup.Clients.FirstOrDefault(x=>x.Client==client)==null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Клиент не является участником сессии!" };
                if (dialog==null)
                {
                    return CreateDialog(model.Sender.Uuid, model.Sender.Type, model.Text, session);
                }
                else
                {
                    return AddMessageInDialog(model.Sender.Uuid, model.Sender.Type,model.Text, dialog);
                }
            }
            else
            {
                var worker = _workersProvider.GetQueryable().FirstOrDefault(x => x.Uuid == model.Sender.Uuid);
                if (worker==null) return new StatusExecution(){ StatusCode = HttpStatusCode.BadRequest, Message = "Работника с таким uuid не существует!" };
                if(dialog==null)
                {
                    return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Нельзя начать диалог!" };
                }
                else
                {
                    return AddMessageInDialog(model.Sender.Uuid, model.Sender.Type, model.Text, dialog);
                }
            }
        }
        private StatusExecution AddMessageInDialog(Guid senderUuid,TypeUser typeSender, string text, Dialog dialog)
        {
            var sender = _participantsProvider.GetQueryable().FirstOrDefault(x => x.Uuid == senderUuid);
            if (sender == null)
            {
                sender = new Participant()
                {
                    Uuid = senderUuid,
                    Type = typeSender
                };
            }
            if (dialog.Participants.FirstOrDefault(x => x.Participant == sender) == null) return new StatusExecution() { StatusCode = HttpStatusCode.BadRequest, Message = "Клиента с таким uuid не участвует в диалоге!" };
            var reads = dialog.Participants.ConvertAll(x => new Read() { UuidParticipant = x.ParticipantUuid});
            reads.FirstOrDefault(x => x.UuidParticipant == senderUuid).IsRead = true;
            dialog.Messages.Add(new Message()
            {
                Text = text,
                Sender = sender,
                Reads = reads
            });
            _dialogsProvider.UpdateAsync(dialog).Wait();
            return new StatusExecution() { StatusCode = HttpStatusCode.OK, Message = "Сообщение успешно отправлено" };
        }
        private StatusExecution CreateDialog(Guid senderUuid, TypeUser typeSender, string text, SessionParking session)
        {
            var sender = _participantsProvider.GetQueryable().FirstOrDefault(x => x.Uuid == senderUuid);
            if (sender == null)
            {
                sender = new Participant()
                {
                    Uuid = senderUuid,
                    Type = typeSender
                };
            }
            var recepient = _participantsProvider.GetQueryable().FirstOrDefault(x => x.Uuid == session.ParkingPlace.Parking.Cluster.Supervisor.Uuid);
            if (recepient == null)
            {
                recepient = new Participant()
                {
                    Type = TypeUser.Worker,
                    Uuid = session.ParkingPlace.Parking.Cluster.Supervisor.Uuid
                };
            }
            _dialogsProvider.InsertAsync(new Dialog()
            {
                Participants = new List<DialogParticipant>()
                        {
                            new DialogParticipant()
                            {
                                Participant = sender
                            },
                            new DialogParticipant()
                            {
                                Participant = recepient
                            }
                        },
                Session = session,
                Messages = new List<Message>()
                        {
                            new Message()
                            {
                                Sender = sender,
                                Text = text,
                                Reads = new List<Read>()
                                {
                                    new Read()
                                    {
                                        UuidParticipant = session.ParkingPlace.Parking.Cluster.Supervisor.Uuid
                                    },
                                    new Read()
                                    {
                                        UuidParticipant = senderUuid,
                                        IsRead =true,
                                    }
                                }
                            }
                        }

            }).Wait();
            return new StatusExecution() { StatusCode = HttpStatusCode.OK, Message = "Сообщение успешно отправлено" };
        }

        /// <summary>
        /// Получение списка диалогов
        /// </summary>
        /// <param name="uuidParticipant">Uuid участника</param>
        /// <returns>Возвращает парковочные места</returns>
        /// <response code="200">Успешная операция</response>
        [HttpGet("get")]
        [ProducesResponseType(typeof(List<OutcomeDialog>), (int)HttpStatusCode.OK)]
        public IActionResult GetFreeParkingPlace([FromHeader] Guid uuidParticipant)
        {
            _log4Net.Info(typeof(ClusterController).ToString(), $"Get all dialog{Environment.NewLine}Participant uuid {uuidParticipant}");
            var dialogs = _dialogsProvider.GetQueryable()
                .Include(x=>x.Session)
                .Include(x => x.Messages).ThenInclude(x => x.Sender)
                .Include(x => x.Messages).ThenInclude(x => x.Reads)
                .Include(x => x.Participants).ThenInclude(x=>x.Participant)
                .Where(x => x.Participants.FirstOrDefault(x => x.Participant.Uuid == uuidParticipant) != null)
                .ToList()
                .ConvertAll(x => x.ToOutcome(uuidParticipant, _workersProvider, _clientProvider));
            _log4Net.Info(typeof(ClusterController).ToString(), $"GGet all dialog{Environment.NewLine}Participant uuid {uuidParticipant}{Environment.NewLine}Status code {HttpStatusCode.OK}");
            return Ok(dialogs);
        }

        /// <summary>
        /// Отправка сообщения о прочтении сообщения
        /// </summary>
        /// <param name="uuidParticipant">Uuid участника</param>
        /// <param name="uuidDialog">Uuid диалога</param>
        /// <returns></returns>
        /// <response code="200">Успешная операция</response>
        [HttpPut("isread")]
        [ProducesResponseType(typeof(List<OutcomeDialog>), (int)HttpStatusCode.OK)]
        public IActionResult GetFreeParkingPlace([FromHeader] Guid uuidParticipant, [FromHeader] Guid uuidDialog)
        {
            _log4Net.Info(typeof(ClusterController).ToString(), $"Get all dialog{Environment.NewLine}Participant uuid {uuidParticipant}");
            var dialog = _dialogsProvider.GetQueryable()
                .Include(x => x.Session)
                .Include(x => x.Messages).ThenInclude(x => x.Sender)
                .Include(x => x.Messages).ThenInclude(x => x.Reads)
                .FirstOrDefault(x => x.Uuid == uuidDialog);
            dialog.Messages.ForEach(x => x.Reads.FirstOrDefault(x => x.UuidParticipant == uuidParticipant).IsRead = true);
            _dialogsProvider.UpdateAsync(dialog).Wait();
            _log4Net.Info(typeof(ClusterController).ToString(), $"GGet all dialog{Environment.NewLine}Participant uuid {uuidParticipant}{Environment.NewLine}Status code {HttpStatusCode.OK}");
            return Ok();
        }
    }
}
