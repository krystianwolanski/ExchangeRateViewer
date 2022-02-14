using ExchangeRateViewer.Application.DTO.External;
using ExchangeRateViewer.Application.Exceptions.ExternalServiceExceptions;
using ExchangeRateViewer.Application.Interfaces;
using ExchangeRateViewer.Application.Interfaces.External;
using ExchangeRateViewer.Infrastructure.ECB_SDMX.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchangeRateViewer.Infrastructure.ECB_SDMX.Services
{
    public class EcbSdmxApiExchangeRatesService : IExternalExchangeRateService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IExchangeRateViewerLog _logger;

        public EcbSdmxApiExchangeRatesService(IHttpClientFactory httpClientFactory, IExchangeRateViewerLog logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRates(
            KeyValuePair<string, string> currencyCodes,
            DateTime startDate,
            DateTime endDate)
        {
            var response = await GetExternalResponse(currencyCodes, startDate, endDate);
            
            var seriesKeys = response.DataSet.Series.SeriesKey;
            var obs = response.DataSet.Series.Obs;
            
            var currencySource = seriesKeys.Single(k => k.id == "CURRENCY").value;
            var currencyTarget = seriesKeys.Single(k => k.id == "CURRENCY_DENOM").value;

            return obs.Select(ob => new ExchangeRateDto
            {
                CurrencySource = currencySource,
                CurrencyTarget = currencyTarget,
                ExchangeRateDate = ob.ObsDimension.value,
                ExchangeRateValue = ob.ObsValue.value
            });
        }

        private async Task<GenericData> GetExternalResponse(
            KeyValuePair<string,string> currencyCodes,
            DateTime startPeriod,
            DateTime endPeriod)
        {
            var fromCurrency = currencyCodes.Key;
            var toCurrency = currencyCodes.Value;
            var startPeriodString = startPeriod.ToString("yyyy-MM-dd");
            var endPeriodString = endPeriod.ToString("yyyy-MM-dd");
            
            var client = _httpClientFactory.CreateClient("ECB_SDMX");

            _logger.Info($"Downloading exchange rates ({fromCurrency}/{toCurrency}) from an external api.\n" +
                $"Start period: {startPeriodString}\n" +
                $"End period: {endPeriodString}");

            var result = await client
                .GetAsync($"D.{fromCurrency}.{toCurrency}.SP00.A" +
                    $"?startPeriod={startPeriodString}" +
                    $"&endPeriod={endPeriodString}" +
                    $"&detail=dataonly");

            var resultContent = await result.Content.ReadAsStringAsync();

            if (!result.IsSuccessStatusCode)
                throw new GetExchangeRatesExternalServiceFailureException("EcbSdmx", result.StatusCode, resultContent);

            if (string.IsNullOrEmpty(resultContent))
            {
                _logger.Info("No exchange rates found.");

                return await GetRecentlyAvailable(currencyCodes, startPeriod);
            }

            var xmlSerializer = new XmlSerializer(typeof(GenericData));
            var genericData = (GenericData)xmlSerializer.Deserialize(new StringReader(resultContent));

            return genericData;
        }

        private async Task<GenericData> GetRecentlyAvailable(
            KeyValuePair<string, string> currencyCodes,
            DateTime startPeriod)
        {
            var tempStartPeriod = startPeriod;
            startPeriod = startPeriod.AddDays(-7);

            _logger.Info($"Downloading recently available.");

            var result = await GetExternalResponse(currencyCodes, startPeriod, tempStartPeriod);
            
            var lastDataElement = result.DataSet.Series.Obs.Last();
            result.DataSet.Series.Obs = new SeriesObs[] { lastDataElement };

            return result;
        }
    }
}
