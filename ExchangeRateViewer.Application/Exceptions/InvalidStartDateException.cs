using System;

namespace ExchangeRateViewer.Application.Exceptions
{
    public class InvalidStartDateException : ExchangeRateViewerException
    {
        public InvalidStartDateException(DateTime date, DateTime currentDate) 
            : base($"The given date - '{date.ToString("yyyy-MM-dd")}' " +
                  $"cannot be later than the current one - '{currentDate.ToString("yyyy-MM-dd")}'")
        {
    
        }
    }
}
