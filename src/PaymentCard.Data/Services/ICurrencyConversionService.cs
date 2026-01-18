using PaymentCard.Domain;

namespace PaymentCard.Data.Services
{
    public interface ICurrencyConversionService
    {
        Task<(decimal? exchangeRateUsed, decimal? convertedAmount, string? targetCurrency, string? errorMessage)> ConvertTransactionsToCurrencyAsync(string targetCurrency, params IEnumerable<PurchaseTransaction> transactions);

        Task<(decimal? exchangeRateUsed, decimal? convertedAmount, string? targetCurrency, string? errorMessage)> ConvertAmountToCurrencyAsync(string targetCurrency, decimal amount, DateTime transactionDate);
    }
}