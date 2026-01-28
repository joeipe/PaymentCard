using PaymentCard.Application.Services.models;

namespace PaymentCard.Application.Services
{
    public interface ICurrencyService
    {
        Task<List<TreasuryExchangeRateDto>?> GetExchangeRatesAsync();
    }
}