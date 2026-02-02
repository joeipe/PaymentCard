namespace PaymentCard.Infrastructure.Network
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> PostAsJsonAsync<T>(HttpClient httpClient, string url, T data);

        Task<HttpResponseMessage> PutAsJsonAsync<T>(HttpClient httpClient, string url, T data);

        Task<T?> ReadContentAsAsync<T>(HttpResponseMessage response);
    }
}