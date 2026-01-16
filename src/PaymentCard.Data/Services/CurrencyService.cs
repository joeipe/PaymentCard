using Microsoft.Extensions.Logging;
using PaymentCard.Data.Services.models;
using SharedKernel.Extensions;
using System.Net;

namespace PaymentCard.Data.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ILogger<CurrencyService> _logger;
        private readonly HttpClient _client;

        public CurrencyService(
            ILogger<CurrencyService> logger,
            HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<List<TreasuryExchangeRateDto>?> GetExchangeRatesAsync()
        {
            _logger.LogInformation("{Class}.{Action} start", nameof(CurrencyService), nameof(GetExchangeRatesAsync));

            var url = "/services/api/fiscal_service/v1/accounting/od/rates_of_exchange";
            var response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                // inspect the status code
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogInformation("The request cannot be found.");
                    return default;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogInformation("Unauthorized");
                    return default;
                }
                response.EnsureSuccessStatusCode();
            }

            var content = await response.Content.ReadAsStringAsync();
            var contacts = content.OutputObject<TreasuryApiResponse>();

            return contacts.Data;
        }
    }
}