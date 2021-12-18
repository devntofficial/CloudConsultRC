using CloudConsult.Common.Clients;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace CloudConsult.Common.DependencyInjection
{
    public static class ApiClientDI
    {
        public static IServiceCollection AddCommonApiClient<T>(this IServiceCollection services, string baseUrl) where T : CommonApiClient
        {
            services.AddHttpClient<T>(x => x.BaseAddress = new Uri(baseUrl))
                .AddTransientHttpErrorPolicy(x => x.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)));
            return services;
        }
    }
}
