using CloudConsult.Common.DependencyInjection;
using CloudConsult.Diagnosis.Domain.Services;
using CloudConsult.Diagnosis.Services.MongoDb.Services;

namespace CloudConsult.Diagnosis.Api.Extensions
{
    public class ServiceExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IReportService, ReportService>();
        }
    }
}