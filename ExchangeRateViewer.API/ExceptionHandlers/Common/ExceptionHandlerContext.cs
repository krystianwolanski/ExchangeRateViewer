using ExchangeRateViewer.Application.Exceptions;
using ExchangeRateViewer.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExchangeRateViewer.API.ExceptionHandlers.Common
{
    public interface IExceptionHandlerContext
    {
        public void HandleExceptions(ExceptionContext context);
    }

    public sealed class ExceptionHandlerContext : IExceptionHandlerContext
    {
        private readonly IExchangeRateViewerLog _logger;

        public ExceptionHandlerContext(IExchangeRateViewerLog logger)
        {
            _logger = logger;
        }

        public void HandleExceptions(ExceptionContext context)
        {
            _logger.Error(context.Exception);

            if (HandleByHandlerIfExists(context))
                return;

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleUnknownException(context);
        }

        private bool HandleByHandlerIfExists(ExceptionContext context)
        {
            var exceptionType = context.Exception.GetType();

            var handlerType = typeof(IExceptionHandler<>).MakeGenericType(exceptionType);

            var handler = context.HttpContext.RequestServices.GetService(handlerType);

            if (handler is not null)
            {
                handlerType.GetMethod(nameof(IExceptionHandler<ExchangeRateViewerException>.HandleException))
                    .Invoke(handler, new object[] { context, context.Exception });

                return true;
            }

            return false;
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private void HandleUnknownException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
