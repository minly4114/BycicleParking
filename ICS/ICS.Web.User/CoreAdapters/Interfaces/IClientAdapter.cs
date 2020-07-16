using ICS.Core.Dtos.Income;
using ICS.Core.Dtos.Income.Client;
using ICS.Core.Dtos.Outcome;
using ICS.Core.Dtos.OutcomeClient;
using ICS.Core.HelperEntities;
using System.Collections.Generic;
using System.Net;

namespace ICS.Web.User.CoreAdapters.Interfaces
{
    public interface IClientAdapter
    {
        Result<OutcomeClient> GetClientInfo(string uuidClient);
        HttpStatusCode UpdateSettingsClient(IncomeClient incomeClient);
        Result<List<OutcomeClientParking>> GetParkings(string parkingUuid = null);
        Result<List<OutcomeClientShortServiceGroup>> GetShortServiceGroup(string uuidClient);
        StatusExecution AddReservation(IncomeClientReservation reservationData);
        Result<OutcomeClientShortInfo> GetClientShort(string cardNumber);
        StatusExecution AddServiceGroup(IncomeClientServiceGroup groupData);
        Result<List<OutcomeClientServiceGroup>> GetServiceGroup(string uuidClient);
        StatusExecution ChangeServiceGroup(string uuidServiceGroup, IncomeClientServiceGroup groupData);
        StatusExecution DeleteServiceGroup(string uuidServiceGroup, string uuidCreator);
        Result<List<OutcomeClientSession>> GetSession(string uuidClient);
        StatusExecution CancelSession(string uuidSession, string uuidClient);
        StatusExecution SendMessage(IncomeMessage message);
        Result<List<OutcomeDialog>> GetDialogs(string uuidUser);
        StatusExecution SendStatusIsRead(string uuidUser,string uuidDialog);
    }
}
