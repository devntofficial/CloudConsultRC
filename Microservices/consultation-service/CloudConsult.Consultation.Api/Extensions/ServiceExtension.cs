using CloudConsult.Common.DependencyInjection;
using CloudConsult.Consultation.Domain.Services;
using CloudConsult.Consultation.Services.SqlServer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Consultation.Api.Extensions
{
    public class ServiceExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAvailabilityService, AvailabilityService>();
            services.AddScoped<IConsultationService, ConsultationService>();
            services.AddScoped<IConsultationEventService, ConsultationEventService>();
        }
    }
}