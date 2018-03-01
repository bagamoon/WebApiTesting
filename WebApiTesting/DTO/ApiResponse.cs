using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WebApiTesting.DTO
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public object Result { get; set; }

        public object Error { get; set; }
    }
}