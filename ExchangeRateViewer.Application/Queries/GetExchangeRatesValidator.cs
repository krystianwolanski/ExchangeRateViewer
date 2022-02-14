using FluentValidation;

namespace ExchangeRateViewer.Application.Queries
{
    public class GetExchangeRatesValidator : AbstractValidator<GetExchangeRates>
    {
        public GetExchangeRatesValidator()
        {
            RuleFor(exchangeRate => exchangeRate.StartDate)
                .LessThanOrEqualTo(exchangeRate => exchangeRate.EndDate)
                .WithMessage($"Start date cannot be later than end date");

            RuleFor(exchangeRate => exchangeRate.EndDate)
                .GreaterThanOrEqualTo(exchangeRate => exchangeRate.StartDate)
                .WithMessage("End date cannot be earlier than start date");
        }
    }
}
