using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentCard.Application.Services;
using PaymentCard.Infrastructure.Currency;
using System.Net;
using System.Text;

namespace PaymentCard.UnitTests.Services
{
    public class CurrencyServiceShould
    {
        private readonly Mock<ILogger<CurrencyService>> _mockLogger = new();

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsData_OnSuccess()
        {
            // Arrange
            var json = @"{
                ""data"": [
                    {
                        ""record_date"": ""2026-01-01T00:00:00Z"",
                        ""country"": ""Algeria"",
                        ""currency"": ""Dinar"",
                        ""country_currency_desc"": ""Algeria-Dinar"",
                        ""exchange_rate"": ""140.0"",
                        ""effective_date"": ""2026-01-01T00:00:00Z"",
                        ""src_line_nbr"": ""1""
                    }
                ]
            }";

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            using var httpClient = new HttpClient(new TestMessageHandler(response))
            {
                BaseAddress = new Uri("https://test")
            };

            var sut = new CurrencyService(_mockLogger.Object, httpClient);

            // Act
            var result = await sut.GetExchangeRatesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            var dto = result![0];
            dto.CountryCurrencyDescription.Should().Be("Algeria-Dinar");
            dto.ExchangeRate.Should().Be(140.0m);
        }

        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsNull_OnNotFound()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.NotFound);

            using var httpClient = new HttpClient(new TestMessageHandler(response))
            {
                BaseAddress = new Uri("https://test")
            };

            var sut = new CurrencyService(_mockLogger.Object, httpClient);

            // Act
            var result = await sut.GetExchangeRatesAsync();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetExchangeRatesAsync_Throws_OnServerError()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);

            using var httpClient = new HttpClient(new TestMessageHandler(response))
            {
                BaseAddress = new Uri("https://test")
            };

            var sut = new CurrencyService(_mockLogger.Object, httpClient);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => sut.GetExchangeRatesAsync());
        }

        private sealed class TestMessageHandler : HttpMessageHandler
        {
            private readonly HttpResponseMessage _response;

            public TestMessageHandler(HttpResponseMessage response)
            {
                _response = response;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                // Return a cloned response to avoid disposal issues across multiple calls
                var clone = new HttpResponseMessage(_response.StatusCode)
                {
                    Content = _response.Content is null ? null : new StringContent(_response.Content.ReadAsStringAsync().Result, Encoding.UTF8, _response.Content.Headers.ContentType?.MediaType),
                    ReasonPhrase = _response.ReasonPhrase,
                    RequestMessage = request,
                    Version = _response.Version
                };

                foreach (var header in _response.Headers)
                {
                    clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                return Task.FromResult(clone);
            }
        }
    }
}