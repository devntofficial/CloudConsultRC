using CloudConsult.Doctor.Domain.Services;
using CloudConsult.Doctor.Services.MongoDb.Services;
using CloudConsult.Common.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Doctor.Api.Extensions
{
    public class ServiceExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IDoctorEventService, DoctorEventService>();
        }
    }
}