using CloudConsult.Common.DependencyInjection;
using CloudConsult.Patient.Api.Configurations;
using CloudConsult.Patient.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CloudConsult.Patient.Api.Extensions
{
    public class MongoDbExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var mongoDbConfiguration = new MongoDbConfiguration();
            configuration.Bind(nameof(mongoDbConfiguration), mongoDbConfiguration);

            var connectionString = $"mongodb://{mongoDbConfiguration.HostName}:{mongoDbConfiguration.Port}";

            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(mongoDbConfiguration.Database);
            var patientCollection = database.GetCollection<PatientEntity>("Patients");

            services.AddSingleton(client);
            services.AddTransient(x => database);
            services.AddTransient(x => patientCollection);
        }
    }
}
