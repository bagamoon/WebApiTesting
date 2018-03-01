using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace WebApiTesting.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        [HttpPost]
        public object GetToken(ValidateDto validateDto)
        {
            var key = "WebApiTesting";

            if (validateDto.Account == validateDto.Password)
            {
                var token = new ApiToken
                {
                    Id = validateDto.Account,
                    ExpiredTime = DateTime.Now.AddMinutes(30)
                    //ExpiredTime = DateTime.Now.AddSeconds(5)
                };

                return new
                {
                    Result = true,
                    token = Jose.JWT.Encode(token, 
                                            Encoding.UTF8.GetBytes(key), 
                                            Jose.JwsAlgorithm.HS256)
                };
            }
            else
            {
                return Unauthorized();
            }
        }
    }

    public class ValidateDto
    {
        public string Account { get; set; }

        public string Password { get; set; }
    }

    public class ApiToken
    {
        public string Id { get; set; }

        public DateTimeOffset ExpiredTime { get; set; }
    }
}
