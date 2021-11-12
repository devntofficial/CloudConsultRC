using CloudConsult.Doctor.Infrastructure.Mappers;
using CloudConsult.Common.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Doctor.Api.Extensions
{
    public class AutoMapperExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(x => { x.AddProfile<DoctorMapper>(); });
        }
    }
}