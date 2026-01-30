using PaymentCard.Application.Models;

namespace PaymentCard.Application.Interfaces.Infrastructure
{
    public interface ICurrencyService
    {
        Task<List<TreasuryExchangeRateDto>?> GetExchangeRatesAsync();
    }
}