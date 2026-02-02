using Microsoft.Extensions.Logging;
using SharedKernel.Extensions;
using System.Net;
using System.Net.Http.Headers;

namespace PaymentCard.Infrastructure.Network
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        private readonly ILogger<HttpClientWrapper> _logger;

        public HttpClientWrapper(ILogger<HttpClientWrapper> logger)
        {
            _logger = logger;
        }

        public Task<HttpResponseMessage> PostAsJsonAsync<T>(HttpClient httpClient, string url, T data)
        {
            var dataAsString = data.OutputJson();
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PostAsync(url, content);
        }

        public Task<HttpResponseMessage> PutAsJsonAsync<T>(HttpClient httpClient, string url, T data)
        {
            var dataAsString = data.OutputJson();
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return httpClient.PutAsync(url, content);
        }

        public async Task<T?> ReadContentAsAsync<T>(HttpResponseMessage response)
        {
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
                else if (response.StatusCode == HttpStatusCode.UnprocessableContent)
                {
                    _logger.LogInformation("UnprocessableContent");
                    return default;
                }
                response.EnsureSuccessStatusCode();
            }

            var dataAsString = await response.Content.ReadAsStringAsync();

            return dataAsString.OutputObject<T>();
        }
    }
}