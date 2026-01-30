using FluentAssertions;
using PaymentCard.Contracts.PurchaseTransactions;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PaymentCard.IntegrationTests.Endpoints
{
    public class TransactionsEndpointTests : IClassFixture<ApiWebApplicationFactory>
    {
        protected readonly ApiWebApplicationFactory _factory;
        protected readonly HttpClient _client;

        public TransactionsEndpointTests(
            ApiWebApplicationFactory fixture)
        {
            _factory = fixture;
            _client = _factory.CreateClient();
        }

        [Theory]
        [InlineData("/transactions")]
        public async Task GET_get_transactions(string url)
        {
            // Act
            var response = await _client.GetAsync(url);
            var dataAsString = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<TransactionBaseResponse>>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/transactions/1")]
        public async Task GET_get_transaction_by_id(string url)
        {
            // Act
            var response = await _client.GetAsync(url);
            var dataAsString = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<TransactionResponse>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/transactions")]
        public async Task POST_create_transaction(string url)
        {
            // Arrange
            var data = new CreateTransactionRequest
            {
                CardId = 1,
                Description = "Integration test transaction",
                TransactionDate = DateTime.Now,
                Amount = 25,
            };

            // Act
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}