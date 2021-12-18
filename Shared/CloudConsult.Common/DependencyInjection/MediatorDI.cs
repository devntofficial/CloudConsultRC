using CloudConsult.Common.Builders;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CloudConsult.Common.DependencyInjection
{
    public static class MediatorDI
    {
        public static IServiceCollection AddCommonMediatorConfiguration(this IServiceCollection services, params string[] projectNames)
        {
            var assemblies = projectNames.Select(Assembly.Load).ToArray();
            services.AddMediatR(assemblies);
            services.AddTransient(typeof(IApiResponseBuilder<>), typeof(ApiResponseBuilder<>));
            services.AddTransient<IApiResponseBuilder, ApiResponseBuilder>();
            return services;
        }
    }
}
