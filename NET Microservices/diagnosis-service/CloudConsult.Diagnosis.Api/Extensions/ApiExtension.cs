using CloudConsult.Common.DependencyInjection;
using System.Text.Json.Serialization;

namespace CloudConsult.Diagnosis.Api.Extensions
{
    public class ApiExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
            });
            services.AddCors(o => o.AddPolicy("DiagnosisServicePolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));
        }
    }
}