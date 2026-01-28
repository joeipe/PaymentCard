using SharedKernel.Services;
using System.Text.Json.Serialization;

namespace PaymentCard.Application.Services.models
{
    public class TreasuryExchangeRateDto
    {
        [JsonPropertyName("record_date")]
        public DateTime RecordDate { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; } = default!;

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = default!;

        [JsonPropertyName("country_currency_desc")]
        public string CountryCurrencyDescription { get; set; } = default!;

        [JsonPropertyName("exchange_rate")]
        [JsonConverter(typeof(StringToDecimalConverter))]
        public decimal ExchangeRate { get; set; }

        [JsonPropertyName("effective_date")]
        public DateTime EffectiveDate { get; set; }

        [JsonPropertyName("src_line_nbr")]
        [JsonConverter(typeof(StringToIntConverter))]
        public int SourceLineNumber { get; set; }
    }
}