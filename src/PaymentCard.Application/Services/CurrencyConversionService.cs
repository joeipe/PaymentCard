using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces;
using PaymentCard.Domain;

namespace PaymentCard.Application.Services
{
    public class CurrencyConversionService : ICurrencyConversionService
    {
        private readonly ILogger<CurrencyConversionService> _logger;
        private readonly ICurrencyService _currencyService;

        public CurrencyConversionService(
            ILogger<CurrencyConversionService> logger,
            ICurrencyService currencyService)
        {
            _logger = logger;
            _currencyService = currencyService;
        }

        public async Task<(decimal? exchangeRateUsed, decimal? convertedAmount, string? targetCurrency, string? errorMessage)> ConvertTransactionsToCurrencyAsync(
            string targetCurrency,
            params IEnumerable<PurchaseTransaction> transactions)
        {
            var exchangeRateResult = await _currencyService.GetExchangeRatesAsync();
            var convertedAmount = 0.0m;

            decimal? exchangeRateUsed = default;
            foreach (var transaction in transactions)
            {
                var cutoffDate = transaction.TransactionDate.AddMonths(-6);

                var selectedRate = exchangeRateResult?
                    .Where(r => r.CountryCurrencyDescription.ToLower() == targetCurrency.ToLower())
                    .Where(r => r.RecordDate <= transaction.TransactionDate)
                    .Where(r => r.RecordDate >= cutoffDate)
                    .OrderByDescending(r => r.RecordDate)
                    .FirstOrDefault();

                convertedAmount += selectedRate is not null ? Math.Round(transaction.Amount * selectedRate.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0.0m;

                if (selectedRate is null)
                {
                    convertedAmount = 0.0m;
                    break;
                }
                exchangeRateUsed = selectedRate?.ExchangeRate;
            }

            decimal? convertedAmountResult = convertedAmount != 0.0m ? Math.Round(convertedAmount, 2, MidpointRounding.AwayFromZero) : null;
            string? targetCurrencyResult = targetCurrency.ToUpperInvariant();
            string? errorMessageResult = convertedAmount == 0.0m ? $"No exchange rates found for currency {targetCurrency} within 6 months prior to transaction dates." : null;

            return (exchangeRateUsed, convertedAmountResult, targetCurrencyResult, errorMessageResult);
        }

        public async Task<(decimal? exchangeRateUsed, decimal? convertedAmount, string? targetCurrency, string? errorMessage)> ConvertAmountToCurrencyAsync(
            string targetCurrency,
            decimal amount,
            DateTime transactionDate)
        {
            var exchangeRateResult = await _currencyService.GetExchangeRatesAsync();

            var cutoffDate = transactionDate.AddMonths(-6);

            var selectedRate = exchangeRateResult?
                .Where(r => r.CountryCurrencyDescription.ToLower() == targetCurrency.ToLower())
                .Where(r => r.RecordDate <= transactionDate)
                .Where(r => r.RecordDate >= cutoffDate)
                .OrderByDescending(r => r.RecordDate)
                .FirstOrDefault();

            var convertedAmount = selectedRate is not null ? Math.Round(amount * selectedRate.ExchangeRate, 2, MidpointRounding.AwayFromZero) : 0.0m;

            decimal? exchangeRateUsed = selectedRate?.ExchangeRate;
            decimal? convertedAmountResult = convertedAmount != 0.0m ? Math.Round(convertedAmount, 2, MidpointRounding.AwayFromZero) : null;
            string? targetCurrencyResult = targetCurrency.ToUpperInvariant();
            string? errorMessageResult = convertedAmount == 0.0m ? $"No exchange rates found for currency {targetCurrency} within 6 months prior to transaction dates." : null;

            return (exchangeRateUsed, convertedAmountResult, targetCurrencyResult, errorMessageResult);
        }
    }
}