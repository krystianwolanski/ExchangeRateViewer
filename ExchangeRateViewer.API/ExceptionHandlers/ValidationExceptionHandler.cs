using ExchangeRateViewer.API.ExceptionHandlers.Common;
using ExchangeRateViewer.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ShopWithPerfume.WebUI.ExceptionHandlers
{
    public class ValidationExceptionHandler : IExceptionHandler<ValidationException>
    {
        public void HandleException(ExceptionContext context, ValidationException exception)
        {
            var details = new ValidationProblemDetails(exception.Errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }
    }
}
