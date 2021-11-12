using CloudConsult.Common.DependencyInjection;
using CloudConsult.Patient.Infrastructure.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Patient.Api.Extensions
{
    public class AutoMapperExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(x => { x.AddProfile<PatientMapper>(); });
        }
    }
}
