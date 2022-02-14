using ExchangeRateViewer.Application.Interfaces;
using NLog;
using System;

namespace ExchangeRateViewer.Infrastructure.Logger
{
    internal class ExchangeRateViewerLog : IExchangeRateViewerLog
    {
        private static readonly NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Warn(string message)
        {
            logger.Warn(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Error(Exception exp)
        {
            logger.Error(exp);
        }
    }
}
