using CloudConsult.Common.DependencyInjection;
using CloudConsult.Patient.Domain.Services;
using CloudConsult.Patient.Services.MongoDb.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Patient.Api.Extensions
{
    public class ServiceExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPatientService, PatientService>();
        }
    }
}
