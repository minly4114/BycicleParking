using ICS.Core.Dtos.Outcome;
using ICS.Web.CoreAdapters.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ICS.Core.Dtos.Income;
using System.Net.Http.Headers;
using System.Text;
using ICS.Core.HelperEntities;

namespace ICS.Web.CoreAdapters.Realization
{
    public class WorkerAdapter : IWorkerAdapter
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private string _request;

        public WorkerAdapter(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            var section = _configuration.GetSection("ICSCore");
            _request = $"{section.GetSection("IpAddress").Value}:{section.GetSection("Port").Value}";
            _clientFactory = clientFactory;
            
        } 
        public HttpStatusCode ConfirmCluster(bool confirm,string uuid)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_request}/api/v1/supervisor/confirm/cluster?confirm={confirm}");
            request.Headers.Add("uuidCluster", uuid);
            using (var client = _clientFactory.CreateClient())
            {
                HttpResponseMessage response = client.SendAsync(request).Result;
                return response.StatusCode;
            }
        }

        public Result<List<OutcomeCluster>> GetClusters(string supervisorUuid = null, string clusterUuid = null)
        {
            var result = new Result<List<OutcomeCluster>>();
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "clusters");
                    if (supervisorUuid != null) request.Headers.Add("supervisorUuid", supervisorUuid);
                    if (clusterUuid != null) request.Headers.Add("clusterUuid", clusterUuid);
                    client.BaseAddress = new Uri($"{_request}/api/v1/supervisor/");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    result.StatusCode = response.StatusCode;
                    result.InternalObject = JsonConvert.DeserializeObject<List<OutcomeCluster>>(responseBody);
                }
            }
            catch
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }

        public Result<OutcomeWorker> GetSupervisor(string uuidSupervisor,bool includeClusters)
        {
            using(var client = _clientFactory.CreateClient())
            {
                client.BaseAddress = new Uri($"{_request}/api/v1/supervisor/");
                var request = new HttpRequestMessage(HttpMethod.Get, "get");
                request.Headers.Add("uuidSupervisor", uuidSupervisor);
                request.Headers.Add("includeClusters", includeClusters.ToString());
                HttpResponseMessage response = client.SendAsync(request).Result;
                Result<OutcomeWorker> result = new Result<OutcomeWorker>()
                {
                    StatusCode = response.StatusCode
                };
                string responseBody = response.Content.ReadAsStringAsync().Result;
                if (response.StatusCode==HttpStatusCode.OK)
                {
                    result.InternalObject = JsonConvert.DeserializeObject<OutcomeWorker>(responseBody);
                }
                return result;
            }
        }

        public HttpStatusCode RegisterSupervisor(IncomeWorker incomeSupervisor)
        {
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "add");
                client.BaseAddress = new Uri($"{_request}/api/v1/supervisor/");
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(JsonConvert.SerializeObject(incomeSupervisor),
                                    Encoding.UTF8,
                                    "application/json");
                HttpResponseMessage response = client.SendAsync(request).Result;
                return response.StatusCode;
            }
        }

        public Result<List<OutcomeParkingPlace>> GetParkingPlaces(string supervisorUuid = null, string parkingUuid = null)
        {
            var result = new Result<List<OutcomeParkingPlace>>();
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "parking/places");
                    if (supervisorUuid != null) request.Headers.Add("supervisorUuid", supervisorUuid);
                    if (parkingUuid != null) request.Headers.Add("parkingUuid", parkingUuid);
                    client.BaseAddress = new Uri($"{_request}/api/v1/supervisor/");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    result.StatusCode = response.StatusCode;
                    result.InternalObject = JsonConvert.DeserializeObject<List<OutcomeParkingPlace>>(responseBody);
                }
            }
            catch
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }

        public Result<List<OutcomeParking>> GetParkings(string supervisorUuid=null, string clusterUuid=null, string parkingUuid = null)
        {
            var result = new Result<List<OutcomeParking>>();
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "parkings");
                    if(supervisorUuid != null) request.Headers.Add("supervisorUuid", supervisorUuid);
                    if (clusterUuid != null) request.Headers.Add("clusterUuid", clusterUuid);
                    if (parkingUuid != null) request.Headers.Add("parkingUuid", parkingUuid);
                    client.BaseAddress = new Uri($"{_request}/api/v1/supervisor/");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    result.StatusCode = response.StatusCode;
                    result.InternalObject = JsonConvert.DeserializeObject<List<OutcomeParking>>(responseBody);
                }
            }
            catch
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }
        public HttpStatusCode UpdateCluster(IncomeCluster incomeCluster)
        {
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "cluster/update");
                client.BaseAddress = new Uri($"{_request}/api/v1/supervisor/");
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(JsonConvert.SerializeObject(incomeCluster),
                                    Encoding.UTF8,
                                    "application/json");
                HttpResponseMessage response = client.SendAsync(request).Result;
                return response.StatusCode;
            }
        }

        public Result<OutcomeParkingConfiguration> GetParkingConfiguration(string parkingUuid)
        {
            var result = new Result<OutcomeParkingConfiguration>();
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "parking/configuration");
                    if (parkingUuid != null) request.Headers.Add("parkingUuid", parkingUuid);
                    client.BaseAddress = new Uri($"{_request}/api/v1/supervisor/");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    result.StatusCode = response.StatusCode;
                    result.InternalObject = JsonConvert.DeserializeObject<OutcomeParkingConfiguration>(responseBody);
                }
            }
            catch
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }

        public HttpStatusCode UpdateParkingConfiguration(IncomeParkingConfiguration parkingConfiguration)
        {
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "parking/configuration");
                client.BaseAddress = new Uri($"{_request}/api/v1/supervisor/");
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(JsonConvert.SerializeObject(parkingConfiguration),
                                    Encoding.UTF8,
                                    "application/json");
                HttpResponseMessage response = client.SendAsync(request).Result;
                return response.StatusCode;
            }
        }

        public Result<List<OutcomeParkingPlaceKeepAlive>> GetParkingPlaceKeepAlives(string parkingPlaceUuid)
        {
            var result = new Result<List<OutcomeParkingPlaceKeepAlive>>();
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "parking/place/keepAlives");
                    request.Headers.Add("parkingPlaceUuid", parkingPlaceUuid);
                    client.BaseAddress = new Uri($"{_request}/api/v1/supervisor/");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    result.StatusCode = response.StatusCode;
                    result.InternalObject = JsonConvert.DeserializeObject<List<OutcomeParkingPlaceKeepAlive>>(responseBody);
                }
            }
            catch
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }

        public StatusExecution SendMessage(IncomeMessage message)
        {
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "send");
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(JsonConvert.SerializeObject(message),
                                    Encoding.UTF8,
                                    "application/json");
                client.BaseAddress = new Uri($"{_request}/api/v1/messages/");
                HttpResponseMessage response = client.SendAsync(request).Result;

                return new StatusExecution() { StatusCode = response.StatusCode, Message = response.Content.ReadAsStringAsync().Result };
            }
        }

        public Result<List<OutcomeDialog>> GetDialogs(string uuidUser)
        {
            var result = new Result<List<OutcomeDialog>>();
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "get");
                    request.Headers.Add("uuidParticipant", uuidUser);
                    client.BaseAddress = new Uri($"{_request}/api/v1/messages/");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    result.StatusCode = response.StatusCode;
                    result.InternalObject = JsonConvert.DeserializeObject<List<OutcomeDialog>>(responseBody);
                }
            }
            catch
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }

        public StatusExecution SendStatusIsRead(string uuidUser, string uuidDialog)
        {
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Put, "isread");
                request.Headers.Add("uuidParticipant", uuidUser);
                request.Headers.Add("uuidDialog", uuidDialog);
                client.BaseAddress = new Uri($"{_request}/api/v1/messages/");
                HttpResponseMessage response = client.SendAsync(request).Result;

                return new StatusExecution() { StatusCode = response.StatusCode, Message = response.Content.ReadAsStringAsync().Result };
            }
        }
    }
}
