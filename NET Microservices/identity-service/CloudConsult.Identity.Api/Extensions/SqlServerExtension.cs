using System.Text;
using CloudConsult.Common.DependencyInjection;
using CloudConsult.Identity.Domain.Configurations;
using CloudConsult.Identity.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Identity.Api.Extensions
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
            
            services.AddDbContext<IdentityDbContext>(x => x.UseSqlServer(builder.ToString()));
        }
    }
}