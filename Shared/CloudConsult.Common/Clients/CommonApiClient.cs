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

        public async Task<IApiResponse<T>> GetAsync<T>(string url, CancellationToken token = default)
        {
            var response = await client.GetAsync(url, token);
            var json = await response.Content.ReadFromJsonAsync<ApiResponseBuilder<T>>(new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }, token);
            return json;
        }
    }
}
