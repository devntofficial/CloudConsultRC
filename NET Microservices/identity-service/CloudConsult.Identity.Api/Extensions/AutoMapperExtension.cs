using CloudConsult.Common.DependencyInjection;
using CloudConsult.Identity.Infrastructure.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Identity.Api.Extensions
{
    public class AutoMapperExtension: IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(x =>
            {
                x.AddProfile<IdentityMapper>();
            });
        }
    }
}