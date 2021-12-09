using CloudConsult.Common.DependencyInjection;
using CloudConsult.Doctor.Infrastructure.Mappers;

namespace CloudConsult.Doctor.Api.Extensions
{
    public class AutoMapperExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(x => { x.AddProfile<ProfileMapper>(); });
        }
    }
}