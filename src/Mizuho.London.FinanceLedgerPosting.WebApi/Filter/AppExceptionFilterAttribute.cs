using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using Mizuho.London.Common.Logging;

namespace Mizuho.London.FinanceLedgerPosting.WebApi.Filter
{
    public class AppExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private IMizLog _logService;

        public AppExceptionFilterAttribute()
        {
            _logService = new MizLog();
        }

        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Exception ex = actionExecutedContext.Exception;
            HttpRequestMessage request = actionExecutedContext.Request;

            _logService.Error(ex.InnerException == null ? ex.Message : ex.InnerException.Message, ex);

            HttpError httpError = new HttpError { Message = ex.Message };
            actionExecutedContext.Response = request.CreateErrorResponse(HttpStatusCode.InternalServerError, httpError);
        }
    }
}