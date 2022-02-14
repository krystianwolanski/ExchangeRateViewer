using ExchangeRateViewer.Application.Configuration.Models;
using ExchangeRateViewer.Application.DTO.External;
using ExchangeRateViewer.Application.Exceptions;
using ExchangeRateViewer.Application.Interfaces;
using ExchangeRateViewer.Shared.Abstractions.Queries;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ValidationException = ExchangeRateViewer.Application.Exceptions.ValidationException;

namespace ExchangeRateViewer.Application.Queries
{
    public sealed record GetExchangeRates(Dictionary<string, string> CurrencyCodes, DateTime StartDate, DateTime EndDate) 
            : IQuery<IEnumerable<ExchangeRateDto>>;

    internal sealed class GetExchangeRatesHandler : IQueryHandler<GetExchangeRates, IEnumerable<ExchangeRateDto>>
    {
        private readonly ExchangeRateSettings _exchangeRateSettings;
        private readonly IExchangeRateInMemoryCacheService _exchangeRateInMemoryCache;
        private readonly IExchangeRateDistributedCacheService _exchangeRateDistributedCache;
        private readonly IValidator<GetExchangeRates> _exchangeRatesValidator;
        private readonly IExchangeRateViewerLog _logger;

        public GetExchangeRatesHandler(ExchangeRateSettings exchangeRateSettings,
            IExchangeRateInMemoryCacheService exchangeRateInMemoryCache,
            IExchangeRateDistributedCacheService exchangeRateDistributedCache,
            IValidator<GetExchangeRates> exchangeRatesValidator,
            IExchangeRateViewerLog logger)
        {
            _exchangeRateSettings = exchangeRateSettings;
            _exchangeRateInMemoryCache = exchangeRateInMemoryCache;
            _exchangeRateDistributedCache = exchangeRateDistributedCache;
            _exchangeRatesValidator = exchangeRatesValidator;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRateDto>> HandleAsync(GetExchangeRates query)
        {
            _logger.Info("Start getting exchange rates.\n" +
                $"Currencies: {string.Join(", ", query.CurrencyCodes)}\n" +
                $"Start date: {query.StartDate}\n" +
                $"End date: {query.EndDate}");

            await ValidateAndThrowAsync(query);
            
            var (currencyCodes, startDate, endDate) = query;

            var passedDays = (endDate - startDate).TotalDays;

            IEnumerable<ExchangeRateDto> result;
            if (passedDays <= _exchangeRateSettings.MaxPassedDays_InMemoryCache
                && currencyCodes.Count <= _exchangeRateSettings.MaxCurrencyConversions_InMemoryCache)
            {
                result =  await _exchangeRateInMemoryCache
                    .GetOrCreateExchangeRatesFromMemoryCache(currencyCodes, startDate, endDate);
            }
            else
            {
                result = await _exchangeRateDistributedCache
                    .GetOrCreateExchangeRatesFromDistributedCache(currencyCodes, startDate, endDate);
            }

            _logger.Info("Returning result");
            return result;
        }

        private async Task ValidateAndThrowAsync(GetExchangeRates query)
        {
            _logger.Info("Validating query.");

            var validateResult = await _exchangeRatesValidator.ValidateAsync(query);
            if (!validateResult.IsValid)
                throw new ValidationException(validateResult.Errors);

            if (query.StartDate > DateTime.UtcNow)
                throw new InvalidStartDateException(query.StartDate, DateTime.UtcNow);
        }
    }
}
