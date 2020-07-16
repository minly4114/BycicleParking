using ICS.Core.Dtos.Income;
using ICS.Core.Dtos.Income.Client;
using ICS.Core.Dtos.Outcome;
using ICS.Core.Dtos.OutcomeClient;
using ICS.Core.HelperEntities;
using ICS.Web.User.CoreAdapters.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ICS.Web.User.CoreAdapters.Realization
{
    public class ClientAdapter : IClientAdapter
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private string _request;
        public ClientAdapter(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            var section = _configuration.GetSection("ICSCore");
            _request = $"{section.GetSection("IpAddress").Value}:{section.GetSection("Port").Value}";
            _clientFactory = clientFactory;
        }
        public Result<OutcomeClient> GetClientInfo(string uuidClient)
        {
            var result = new Result<OutcomeClient>();
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "getClientInfo");
                    request.Headers.Add("uuidClient", uuidClient);
                    client.BaseAddress = new Uri($"{_request}/api/v1/client/");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    result.StatusCode = response.StatusCode;
                    result.InternalObject = JsonConvert.DeserializeObject<OutcomeClient>(responseBody);
                }
            }
            catch
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }

        public HttpStatusCode UpdateSettingsClient(IncomeClient incomeClient)
        {
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "updateClientInfo");
                client.BaseAddress = new Uri($"{_request}/api/v1/client/");
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(JsonConvert.SerializeObject(incomeClient),
                                    Encoding.UTF8,
                                    "application/json");
                HttpResponseMessage response = client.SendAsync(request).Result;
                return response.StatusCode;
            }
        }

        public Result<List<OutcomeClientParking>> GetParkings(string parkingUuid = null)
        {

            var result = new Result<List<OutcomeClientParking>>();
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "parkings");
                    if (parkingUuid != null) request.Headers.Add("parkingUuid", parkingUuid);
                    client.BaseAddress = new Uri($"{_request}/api/v1/client/");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    result.StatusCode = response.StatusCode;
                    result.InternalObject = JsonConvert.DeserializeObject<List<OutcomeClientParking>>(responseBody);
                }
            }
            catch
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }

        public Result<List<OutcomeClientShortServiceGroup>> GetShortServiceGroup(string uuidClient)
        {
            var result = new Result<List<OutcomeClientShortServiceGroup>> ();
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "groups/short");
                    request.Headers.Add("uuidClient", uuidClient);
                    client.BaseAddress = new Uri($"{_request}/api/v1/client/");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    result.StatusCode = response.StatusCode;
                    result.InternalObject = JsonConvert.DeserializeObject<List<OutcomeClientShortServiceGroup>>(responseBody);
                }
            }
            catch
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }

        public StatusExecution AddReservation(IncomeClientReservation reservationData)
        {
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "place/reservation");
                client.BaseAddress = new Uri($"{_request}/api/v1/client/");
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(JsonConvert.SerializeObject(reservationData),
                                    Encoding.UTF8,
                                    "application/json");
                HttpResponseMessage response = client.SendAsync(request).Result;

                return new StatusExecution() { StatusCode = response.StatusCode, Message = response.Content.ReadAsStringAsync().Result };
            }
        }

        public Result<OutcomeClientShortInfo> GetClientShort(string cardNumber)
        {
            var result = new Result<OutcomeClientShortInfo>();
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "short/info");
                    if (cardNumber != null) request.Headers.Add("cardNumber", cardNumber);
                    client.BaseAddress = new Uri($"{_request}/api/v1/client/");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    result.StatusCode = response.StatusCode;
                    result.InternalObject = JsonConvert.DeserializeObject<OutcomeClientShortInfo>(responseBody);
                }
            }
            catch
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }

        public StatusExecution AddServiceGroup(IncomeClientServiceGroup groupData)
        {
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "servicwGroup/create");
                client.BaseAddress = new Uri($"{_request}/api/v1/client/");
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(JsonConvert.SerializeObject(groupData),
                                    Encoding.UTF8,
                                    "application/json");
                HttpResponseMessage response = client.SendAsync(request).Result;

                return new StatusExecution() { StatusCode = response.StatusCode, Message = response.Content.ReadAsStringAsync().Result };
            }
        }

        public Result<List<OutcomeClientServiceGroup>> GetServiceGroup(string uuidClient)
        {
            var result = new Result<List<OutcomeClientServiceGroup>>();
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "groups");
                    request.Headers.Add("uuidClient", uuidClient);
                    client.BaseAddress = new Uri($"{_request}/api/v1/client/");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    result.StatusCode = response.StatusCode;
                    result.InternalObject = JsonConvert.DeserializeObject<List<OutcomeClientServiceGroup>>(responseBody);
                }
            }
            catch
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }

        public StatusExecution ChangeServiceGroup(string uuidServiceGroup, IncomeClientServiceGroup groupData)
        {
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Put, "group/change");
                request.Headers.Add("uuidServiceGroup", uuidServiceGroup);
                client.BaseAddress = new Uri($"{_request}/api/v1/client/");
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Content = new StringContent(JsonConvert.SerializeObject(groupData),
                                    Encoding.UTF8,
                                    "application/json");
                HttpResponseMessage response = client.SendAsync(request).Result;

                return new StatusExecution() { StatusCode = response.StatusCode, Message = response.Content.ReadAsStringAsync().Result };
            }
        }

        public StatusExecution DeleteServiceGroup(string uuidServiceGroup, string uuidCreator)
        {
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, "group/delete");
                request.Headers.Add("uuidServiceGroup", uuidServiceGroup);
                request.Headers.Add("uuidCreator", uuidCreator);
                client.BaseAddress = new Uri($"{_request}/api/v1/client/");
                HttpResponseMessage response = client.SendAsync(request).Result;

                return new StatusExecution() { StatusCode = response.StatusCode, Message = response.Content.ReadAsStringAsync().Result };
            }
        }

        public Result<List<OutcomeClientSession>> GetSession(string uuidClient)
        {
            var result = new Result<List<OutcomeClientSession>>();
            try
            {
                using (var client = _clientFactory.CreateClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, "sessions");
                    request.Headers.Add("uuidClient", uuidClient);
                    client.BaseAddress = new Uri($"{_request}/api/v1/client/");
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    result.StatusCode = response.StatusCode;
                    result.InternalObject = JsonConvert.DeserializeObject<List<OutcomeClientSession>>(responseBody);
                }
            }
            catch
            {
                result.StatusCode = HttpStatusCode.NotFound;
            }
            return result;
        }

        public StatusExecution CancelSession(string uuidSession, string uuidClient)
        {
            using (var client = _clientFactory.CreateClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Put, "session/cancel");
                request.Headers.Add("uuidSession", uuidSession);
                request.Headers.Add("uuidClient", uuidClient);
                client.BaseAddress = new Uri($"{_request}/api/v1/client/");
                HttpResponseMessage response = client.SendAsync(request).Result;

                return new StatusExecution() { StatusCode = response.StatusCode, Message = response.Content.ReadAsStringAsync().Result };
            }
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
