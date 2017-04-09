using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using System.Threading;
using System.Threading.Tasks;

namespace Centralized.WebAPI.Common
{
    public class GlobalExceptionHandling : ExceptionFilterAttribute
    {

        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }
            else
                context.Response = new HttpResponseMessage(HttpStatusCode.ExpectationFailed);
        }
    }
}