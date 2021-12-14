using CloudConsult.Common.DependencyInjection;
using CloudConsult.Notification.Consumers.Consultation;
using CloudConsult.Notification.Consumers.Diagnosis;
using CloudConsult.Notification.Consumers.Identity;
using Kafka.Public;
using Kafka.Public.Loggers;
using MailKit.Net.Smtp;
using Member = CloudConsult.Notification.Consumers.Member;
using Doctor = CloudConsult.Notification.Consumers.Doctor;

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
            services.AddHostedService<Doctor.ProfileCreatedConsumer>();
            services.AddHostedService<Doctor.ProfileUpdatedConsumer>();
            services.AddHostedService<Doctor.KycApprovedConsumer>();
            services.AddHostedService<Doctor.KycRejectedConsumer>();
            services.AddHostedService<Member.ProfileCreatedConsumer>();
            services.AddHostedService<Member.ProfileUpdatedConsumer>();
            services.AddHostedService<ConsultationRequestedConsumer>();
            services.AddHostedService<ConsultationAcceptedConsumer>();
            services.AddHostedService<ConsultationRejectedConsumer>();
            services.AddHostedService<ConsultationCancelledConsumer>();
            services.AddHostedService<ReportUploadedConsumer>();
        }
    }
}
