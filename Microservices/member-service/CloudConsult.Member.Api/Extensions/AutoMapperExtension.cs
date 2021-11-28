using CloudConsult.Common.DependencyInjection;
using CloudConsult.Member.Infrastructure.Mappers;

namespace CloudConsult.Member.Api.Extensions
{
    public class AutoMapperExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(x => { x.AddProfile<ProfileMapper>(); });
        }
    }
}
