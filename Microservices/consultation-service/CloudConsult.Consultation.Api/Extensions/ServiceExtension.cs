using CloudConsult.Common.DependencyInjection;
using CloudConsult.Consultation.Domain.Services;
using CloudConsult.Consultation.Services.SqlServer.Services;

namespace CloudConsult.Consultation.Api.Extensions;

public class ServiceExtension : IApiStartupExtension
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITimeSlotService, TimeSlotService>();
        services.AddScoped<IConsultationService, ConsultationService>();
        services.AddScoped<IEventService, EventService>();
    }
}