using CloudConsult.Common.DependencyInjection;
using CloudConsult.Diagnosis.Infrastructure.Mappers;

namespace CloudConsult.Diagnosis.Api.Extensions
{
    public class AutoMapperExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(x => {
                x.AddProfile<ReportMapper>();
            });
        }
    }
}