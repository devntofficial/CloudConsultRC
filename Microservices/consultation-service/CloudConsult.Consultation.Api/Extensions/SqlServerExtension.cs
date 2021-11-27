using CloudConsult.Common.DependencyInjection;
using CloudConsult.Consultation.Domain.Configurations;
using CloudConsult.Consultation.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CloudConsult.Consultation.Api.Extensions
{
    public class SqlServerExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var config = new SqlServerConfiguration();
            configuration.Bind(nameof(SqlServerConfiguration), config);

            var builder = new StringBuilder();
            builder.Append($"Server={config.HostName};");
            builder.Append($"Database={config.Database};");
            builder.Append($"User Id={config.Username};Password={config.Password};");
            builder.Append($"MultipleActiveResultSets={config.MultipleActiveResultSets};");

            services.AddDbContext<ConsultationDbContext>(x => x.UseSqlServer(builder.ToString()));
        }
    }
}