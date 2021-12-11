using CloudConsult.Common.DependencyInjection;
using CloudConsult.Notification.Consumers.Consultation;
using CloudConsult.Notification.Consumers.Doctor;
using CloudConsult.Notification.Consumers.Identity;
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
            services.AddHostedService<ProfileCreatedConsumer>();
            services.AddHostedService<ProfileUpdatedConsumer>();
            services.AddHostedService<KycApprovedConsumer>();
            services.AddHostedService<KycRejectedConsumer>();
            services.AddHostedService<ConsultationRequestedConsumer>();
            services.AddHostedService<ConsultationAcceptedConsumer>();
            services.AddHostedService<ConsultationRejectedConsumer>();
            services.AddHostedService<ConsultationCancelledConsumer>();
        }
    }
}
