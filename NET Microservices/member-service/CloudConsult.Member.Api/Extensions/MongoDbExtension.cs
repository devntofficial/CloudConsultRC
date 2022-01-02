using CloudConsult.Common.DependencyInjection;
using CloudConsult.Member.Domain.Configurations;
using CloudConsult.Member.Domain.Entities;
using MongoDB.Driver;

namespace CloudConsult.Member.Api.Extensions
{
    public class MongoDbExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbConfiguration = new MongoDbConfiguration();
            configuration.Bind(nameof(mongoDbConfiguration), mongoDbConfiguration);

            var connectionString = $"mongodb://{mongoDbConfiguration.Username}:{mongoDbConfiguration.Password}@{mongoDbConfiguration.HostName}:{mongoDbConfiguration.Port}/admin";

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(mongoDbConfiguration.Database);
            var doctorCollection = database.GetCollection<MemberProfile>("MemberProfiles");

            services.AddSingleton(client);
            services.AddScoped(x => doctorCollection);
        }
    }
}
