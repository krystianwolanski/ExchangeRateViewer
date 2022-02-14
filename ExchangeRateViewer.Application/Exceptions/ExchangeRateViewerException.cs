using System;

namespace ExchangeRateViewer.Application.Exceptions
{
    public abstract class ExchangeRateViewerException : Exception
    {
        public ExchangeRateViewerException()
        {

        }

        public ExchangeRateViewerException(string message) : base(message)
        {

        }
    }
}
