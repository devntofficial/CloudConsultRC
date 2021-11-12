using CloudConsult.Consultation.Infrastructure.Mappers;
using CloudConsult.Common.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Consultation.Api.Extensions
{
    public class AutoMapperExtension: IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(x =>
            {
                x.AddProfile<AvailabilityMapper>();
                x.AddProfile<ConsultationMapper>();
            });
        }
    }
}