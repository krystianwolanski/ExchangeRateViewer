using ExchangeRateViewer.API.ExceptionHandlers.Common;
using ExchangeRateViewer.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExchangeRateViewer.API.ExceptionHandlers
{
    public class InvalidStartDateExceptionHandler : IExceptionHandler<InvalidStartDateException>
    {
        public void HandleException(ExceptionContext context, InvalidStartDateException exception)
        {
            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception.Message
            };

            context.Result = new NotFoundObjectResult(details);

            context.ExceptionHandled = true;
        }
    }
}
