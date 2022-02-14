using ExchangeRateViewer.API.ExceptionHandlers.Common;
using ExchangeRateViewer.Application.Exceptions.ExternalServiceExceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExchangeRateViewer.API.ExceptionHandlers
{
    public class ExternalServiceFailureExceptionHandler : IExceptionHandler<GetExchangeRatesExternalServiceFailureException>
    {
        public void HandleException(ExceptionContext context, GetExchangeRatesExternalServiceFailureException exception)
        {
            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = "An error occurred while getting data from an external service"
            };

            context.Result = new NotFoundObjectResult(details);

            context.ExceptionHandled = true;
        }
    }
}
