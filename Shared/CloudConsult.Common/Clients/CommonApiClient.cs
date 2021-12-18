using CloudConsult.Common.Builders;
using System.Net.Http.Json;
using System.Text.Json;

namespace CloudConsult.Common.Clients
{
    public class CommonApiClient
    {
        private readonly HttpClient client;

        public CommonApiClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<IApiResponse<TResponse>> GetAsync<TResponse>(string url, CancellationToken token = default)
        {
            var response = await client.GetAsync(url, token);
            var json = await response.Content.ReadFromJsonAsync<ApiResponseBuilder<TResponse>>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }, token);
            return json;
        }

        public async Task<IApiResponse> PostAsync<TRequest>(string url, TRequest data, CancellationToken token = default)
        {
            var response = await client.PostAsJsonAsync(url, data, token);
            var json = await response.Content.ReadFromJsonAsync<ApiResponseBuilder>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }, token);
            return json;
        }

        public async Task<IApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest data, CancellationToken token = default)
        {
            var response = await client.PostAsJsonAsync(url, data, token);
            var json = await response.Content.ReadFromJsonAsync<ApiResponseBuilder<TResponse>>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }, token);
            return json;
        }
    }
}
