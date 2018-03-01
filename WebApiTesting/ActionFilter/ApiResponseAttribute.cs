using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using WebApiTesting.DTO;

namespace WebApiTesting.ActionFilter
{
    public class ApiResponseAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {            
            if (actionExecutedContext.Exception != null)
            {
                return;
            }

            var result = new ApiResponse
            {
                StatusCode = actionExecutedContext.ActionContext.Response.StatusCode,
                Result = actionExecutedContext.ActionContext.Response.Content.ReadAsAsync<object>().Result
            };

            actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(result.StatusCode, result);            

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}