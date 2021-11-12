using CloudConsult.Common.DependencyInjection;
using CloudConsult.Identity.Domain.Services;
using CloudConsult.Identity.Services.SqlServer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Identity.Api.Extensions
{
    public class ServiceExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}