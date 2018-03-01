using Jose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApiTesting.Controllers;

namespace WebApiTesting.ActionFilter
{
    public class TokenCheck : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var secret = "WebApiTesting";

            if (actionContext.Request.Headers.Authorization == null || 
                actionContext.Request.Headers.Authorization.Scheme != "Bearer")
            {
                setErrorResponse(actionContext, "invalid token");
            }
            else
            {
                try
                {
                    var token = JWT.Decode<ApiToken>(
                        actionContext.Request.Headers.Authorization.Parameter,
                        Encoding.UTF8.GetBytes(secret),
                        JwsAlgorithm.HS256);

                    if (DateTime.Now > token.ExpiredTime.LocalDateTime)
                    {
                        setErrorResponse(actionContext, "token expired");
                    }

                }
                catch (Exception ex)
                {
                    setErrorResponse(actionContext, ex.Message);
                }
            }            

            base.OnActionExecuting(actionContext);
        }

        private static void setErrorResponse(HttpActionContext actionContext, string message)
        {
            var response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, message);
            actionContext.Response = response;            
        }

    }
}