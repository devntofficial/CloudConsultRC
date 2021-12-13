using CloudConsult.Common.DependencyInjection;
using CloudConsult.Consultation.Infrastructure.Consumers;
using Kafka.Public;
using Kafka.Public.Loggers;

namespace CloudConsult.Consultation.Api.Extensions
{
    public class ConsumerExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(x => new ClusterClient(new Configuration
            {
                Seeds = configuration["KafkaConfiguration:BootstrapServers"]
            }, new ConsoleLogger()));

            services.AddHostedService<ReportUploadedConsumer>();
        }
    }
}
