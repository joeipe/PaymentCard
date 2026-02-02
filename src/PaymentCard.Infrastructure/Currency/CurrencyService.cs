using Microsoft.Extensions.Logging;
using PaymentCard.Application.Interfaces.Infrastructure;
using PaymentCard.Application.Models;
using PaymentCard.Infrastructure.Network;

namespace PaymentCard.Infrastructure.Currency
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ILogger<CurrencyService> _logger;
        private readonly HttpClient _client;
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly PolicyHolder _policyHolder;

        public CurrencyService(
            ILogger<CurrencyService> logger,
            HttpClient client,
            IHttpClientWrapper httpClientWrapper,
            PolicyHolder policyHolder)
        {
            _logger = logger;
            _client = client;
            _httpClientWrapper = httpClientWrapper;
            _policyHolder = policyHolder;
        }

        public async Task<List<TreasuryExchangeRateDto>?> GetExchangeRatesAsync()
        {
            _logger.LogInformation("{Class}.{Action} start", nameof(CurrencyService), nameof(GetExchangeRatesAsync));

            var url = "/services/api/fiscal_service/v1/accounting/od/rates_of_exchange";
            var response = await _policyHolder.PolicyWrap.ExecuteAsync(() =>
            {
                return _client.GetAsync(url);
            });
            var ratesOfExchange = await _httpClientWrapper.ReadContentAsAsync<TreasuryApiResponse>(response);

            return ratesOfExchange?.Data;
        }
    }
}