using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Common.DependencyInjection
{
    public interface IApiStartupExtension
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);
    }
    
    public static class ApiStartupExtension
    {
        public static IServiceCollection AddCommonExtensionsFromCurrentAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.GetCallingAssembly();
            ConfigureExtensionsFromAssemblies(services, configuration, assembly);
            return services;
        }

        public static IServiceCollection ConfigureExtensionsFromAssemblyContaining<TMarker>(this IServiceCollection services,
            IConfiguration configuration) where TMarker : IApiStartupExtension
        {
            ConfigureExtensionsFromAssembliesContaining(services, configuration, typeof(TMarker));
            return services;
        }

        public static void ConfigureExtensionsFromAssembliesContaining(this IServiceCollection services,
            IConfiguration configuration, params Type[] assemblyMarkers)
        {
            var assemblies = assemblyMarkers.Select(x => x.Assembly).ToArray();
            ConfigureExtensionsFromAssemblies(services, configuration, assemblies);
        }

        private static void ConfigureExtensionsFromAssemblies(this IServiceCollection services,
            IConfiguration configuration, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies)
            {
                var installerTypes = assembly.DefinedTypes
                    .Where(x => typeof(IApiStartupExtension).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract);

                var installers = installerTypes.Select(Activator.CreateInstance).Cast<IApiStartupExtension>();

                foreach (var apiInstaller in installers)
                {
                    apiInstaller.ConfigureServices(services, configuration);
                }
            }
        }
    }
}