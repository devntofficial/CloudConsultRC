using CloudConsult.Doctor.Domain.Configurations;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Common.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CloudConsult.Doctor.Api.Extensions
{
    public class MongoDbExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbConfiguration = new MongoDbConfiguration();
            configuration.Bind(nameof(mongoDbConfiguration), mongoDbConfiguration);

            var connectionString = $"mongodb://{mongoDbConfiguration.HostName}:{mongoDbConfiguration.Port.ToString()}";
            
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(mongoDbConfiguration.Database);
            var doctorCollection = database.GetCollection<DoctorProfile>("DoctorProfiles");

            services.AddSingleton(client);
            services.AddScoped(x => doctorCollection);
        }
    }
}