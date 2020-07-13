using System.Net;

namespace ICS.Core.HelperEntities
{
    public class StatusExecution
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}
