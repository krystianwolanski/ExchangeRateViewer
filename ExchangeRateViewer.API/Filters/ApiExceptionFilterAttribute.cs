using ExchangeRateViewer.API.ExceptionHandlers.Common;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExchangeRateViewer.API.Filters
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IExceptionHandlerContext _exceptionHandlerContext;

        public ApiExceptionFilterAttribute(IExceptionHandlerContext exceptionHandlerContext)
        {
            _exceptionHandlerContext = exceptionHandlerContext;
        }

        public override void OnException(ExceptionContext context)
        {
            _exceptionHandlerContext.HandleExceptions(context);

            base.OnException(context);
        }
    }
}
