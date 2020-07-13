using ICS.Core.Dtos.Outcome;
using Microsoft.AspNetCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ICS.Core.Dtos.Income;
using ICS.Core.HelperEntities;

namespace ICS.Web.CoreAdapters.Interfaces
{
    public interface IWorkerAdapter
    {
        Result<List<OutcomeCluster>> GetClusters(string supervisorUuid = null, string clusterUuid = null);
        HttpStatusCode ConfirmCluster(bool confirm, string uuid);
        Result<List<OutcomeParking>> GetParkings(string supervisorUuid = null, string clusterUuid = null,string parkingUuid = null);
        Result<List<OutcomeParkingPlace>> GetParkingPlaces(string supervisorUuid = null, string parkingUuid = null);
        HttpStatusCode RegisterSupervisor(IncomeWorker incomeSupervisor);
        Result<OutcomeWorker> GetSupervisor(string uuidSupervisor, bool includeClusters);
        HttpStatusCode UpdateCluster(IncomeCluster incomeCluster);
        Result<OutcomeParkingConfiguration> GetParkingConfiguration(string parkingUuid);
        HttpStatusCode UpdateParkingConfiguration(IncomeParkingConfiguration parkingConfiguration);
        Result<List<OutcomeParkingPlaceKeepAlive>> GetParkingPlaceKeepAlives(string parkingPlaceUuid);
        StatusExecution SendMessage(IncomeMessage message);
        Result<List<OutcomeDialog>> GetDialogs(string uuidUser);
        StatusExecution SendStatusIsRead(string uuidUser, string uuidDialog);
    }
}
