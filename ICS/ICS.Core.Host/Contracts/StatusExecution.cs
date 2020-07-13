using System.Net;

namespace ICS.Core.Host.Contracts
{
    public class StatusExecution
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}
