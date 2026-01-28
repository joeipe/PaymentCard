using System.Text.Json.Serialization;

namespace PaymentCard.Application.Services.models
{
    public class TreasuryApiResponse
    {
        [JsonPropertyName("data")]
        public List<TreasuryExchangeRateDto> Data { get; set; } = new();
    }
}