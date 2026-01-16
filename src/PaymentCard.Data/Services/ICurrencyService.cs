using PaymentCard.Data.Services.models;

namespace PaymentCard.Data.Services
{
    public interface ICurrencyService
    {
        Task<List<TreasuryExchangeRateDto>?> GetExchangeRatesAsync();
    }
}