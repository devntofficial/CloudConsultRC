using CloudConsult.Common.Encryption;
using CloudConsult.Common.Middlewares;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Common.DependencyInjection
{
    public static class CommonServicesDI
    {
        public static IServiceCollection AddCommonHashingService(this IServiceCollection services)
        {
            services.AddScoped<IHashingService, HashingService>();
            return services;
        }

        public static IServiceCollection AddCommonMiddlewares(this IServiceCollection services)
        {
            services.AddTransient<GlobalExceptionHandlingMiddleware>();
            return services;
        }
    }
}
