using PaymentCard.Application.Services.models;

namespace PaymentCard.Application.Interfaces.Services
{
    public interface ICurrencyService
    {
        Task<List<TreasuryExchangeRateDto>?> GetExchangeRatesAsync();
    }
}