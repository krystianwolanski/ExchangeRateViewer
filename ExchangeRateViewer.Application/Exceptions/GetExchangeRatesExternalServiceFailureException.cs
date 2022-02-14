using System.Net;

namespace ExchangeRateViewer.Application.Exceptions.ExternalServiceExceptions
{
    public class GetExchangeRatesExternalServiceFailureException : ExchangeRateViewerException
    {
        public string ServiceName { get; }
        public HttpStatusCode StatusCode { get; }
        public string Response { get; }

        public GetExchangeRatesExternalServiceFailureException(string serviceName, HttpStatusCode statusCode, string response)
            : base($"{serviceName} service returned a failure status code")
        {
            ServiceName = serviceName;
            Response = response;
            StatusCode = statusCode;
        }
    }
}
