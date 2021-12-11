using CloudConsult.Common.DependencyInjection;
using CloudConsult.Consultation.Infrastructure.Mappers;

namespace CloudConsult.Consultation.Api.Extensions;

public class AutoMapperExtension : IApiStartupExtension
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(x =>
        {
            x.AddProfile<AvailabilityMapper>();
            x.AddProfile<EventMapper>();
            x.AddProfile<ConsultationMapper>();
        });
    }
}