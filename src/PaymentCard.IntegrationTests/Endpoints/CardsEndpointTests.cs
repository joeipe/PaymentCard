using FluentAssertions;
using PaymentCard.Contracts.Cards;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace PaymentCard.IntegrationTests.Endpoints
{
    public class CardsEndpointTests : IClassFixture<ApiWebApplicationFactory>
    {
        protected readonly ApiWebApplicationFactory _factory;
        protected readonly HttpClient _client;

        public CardsEndpointTests(
            ApiWebApplicationFactory fixture)
        {
            _factory = fixture;
            _client = _factory.CreateClient();
        }

        [Theory]
        [InlineData("/cards")]
        public async Task GET_get_cards(string url)
        {
            // Arrange
            // Act
            var response = await _client.GetAsync(url);
            var dataAsString = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<CardResponse>>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/cards/1")]
        public async Task GET_get_cards_by_id(string url)
        {
            // Arrange
            // Act
            var response = await _client.GetAsync(url);
            var dataAsString = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<CardResponse>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/cards/1/balance")]
        public async Task GET_get_card_balance(string url)
        {
            // Arrange
            // Act
            var response = await _client.GetAsync(url);
            var dataAsString = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<CardBalanceResponse>(dataAsString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/cards")]
        public async Task POST_create_card(string url)
        {
            // Arrange
            var data = new CreateCardRequest
            {
                CardNumber = "4111111111111111",
                CreditLimit = 100,
            };

            // Act
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PostAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("/cards/1")]
        public async Task PUT_update_card(string url)
        {
            // Arrange
            var data = new UpdateCardRequest
            {
                CreditLimit = 50,
            };

            // Act
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await _client.PutAsync(url, content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Theory]
        [InlineData("/cards/2")]
        public async Task DELETE_delete_card(string url)
        {
            // Arrange
            // Act
            var response = await _client.DeleteAsync(url);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}