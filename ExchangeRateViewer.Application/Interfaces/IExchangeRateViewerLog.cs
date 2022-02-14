using System;

namespace ExchangeRateViewer.Application.Interfaces
{
    public interface IExchangeRateViewerLog
    {
        void Info(string message);
        void Warn(string message);
        void Debug(string message);
        void Error(string message);
        void Error(Exception exp);
    }
}
