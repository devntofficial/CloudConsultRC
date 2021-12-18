using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CloudConsult.Common.DependencyInjection
{
    public static class FluentValidationDI
    {
        public static IServiceCollection AddCommonValidationsFrom(this IServiceCollection services, params string[] projectNames)
        {
            var assemblies = projectNames.Select(Assembly.Load).ToArray();
            services.AddValidatorsFromAssemblies(assemblies);
            return services;
        }
    }
}
