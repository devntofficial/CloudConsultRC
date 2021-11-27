using CloudConsult.Common.DependencyInjection;
using CloudConsult.Doctor.Domain.Services;
using CloudConsult.Doctor.Services.MongoDb.Services;

namespace CloudConsult.Doctor.Api.Extensions
{
    public class ServiceExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IEventService, EventService>();
        }
    }
}