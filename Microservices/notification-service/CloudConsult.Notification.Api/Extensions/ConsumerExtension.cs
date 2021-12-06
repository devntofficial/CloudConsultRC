using CloudConsult.Common.DependencyInjection;
using CloudConsult.Notification.Api.Consumers;
using Kafka.Public;
using Kafka.Public.Loggers;
using MailKit.Net.Smtp;

namespace CloudConsult.Notification.Api.Extensions
{
    public class ConsumerExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<SmtpClient>();

            services.AddTransient(x => new ClusterClient(new Configuration
            {
                Seeds = configuration["KafkaConfiguration:BootstrapServers"]
            }, new ConsoleLogger()));

            services.AddHostedService<OtpGeneratedConsumer>();
        }
    }
}
