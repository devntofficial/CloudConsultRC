using CloudConsult.Common.DependencyInjection;
using CloudConsult.Member.Domain.Services;
using CloudConsult.Member.Services.MongoDb.Services;

namespace CloudConsult.Member.Api.Extensions
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