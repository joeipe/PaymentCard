using PaymentCard.Application.Services.models;

namespace PaymentCard.Application.Interfaces
{
    public interface ICurrencyService
    {
        Task<List<TreasuryExchangeRateDto>?> GetExchangeRatesAsync();
    }
}