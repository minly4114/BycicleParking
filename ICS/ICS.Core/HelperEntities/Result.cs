using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ICS.Core.HelperEntities
{
    public class Result<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public T InternalObject { get; set; }
    }
}
