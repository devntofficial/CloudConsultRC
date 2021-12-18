using CloudConsult.Notification.Events.Doctor;
using FluentEmail.Core;
using Kafka.Public;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace CloudConsult.Notification.Consumers.Doctor
{
    public class KycRejectedConsumer : IHostedService
    {
        private readonly ILogger<KycRejectedConsumer> logger;
        private readonly ClusterClient cluster;
        private readonly IServiceProvider serviceProvider;
        private readonly string topicName;
        private readonly string emailTemplate;

        public KycRejectedConsumer(ILogger<KycRejectedConsumer> logger, ClusterClient cluster,
            IServiceProvider serviceProvider, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.serviceProvider = serviceProvider;
            topicName = config["KafkaConfiguration:ConsumerTopics:DoctorKycRejected"];
            this.emailTemplate = "Templates/Doctor/KycRejectedEmail.cshtml";
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cluster.ConsumeFromLatest(topicName);
            cluster.MessageReceived += async record => await ProcessMessage(record);
            return Task.CompletedTask;
        }

        public async Task ProcessMessage(RawKafkaRecord record)
        {
            try
            {
                if (record is null || record.Value is null)
                {
                    logger.LogError($"Invalid message consumed from topic: {topicName}");
                    return;
                }

                if (record.Value is not byte[] byteArray)
                {
                    logger.LogError($"Invalid message payload consumed from topic: {topicName}");
                    return;
                }

                using var memoryStream = new MemoryStream(byteArray);
                using var reader = new StreamReader(memoryStream, Encoding.UTF8);

                var kycRejectedEvent = await JsonSerializer.DeserializeAsync<KycRejected>(reader.BaseStream);
                if (kycRejectedEvent is null)
                {
                    logger.LogError($"Invalid message format consumed from topic: {topicName}");
                    return;
                }

                using var scope = serviceProvider.CreateScope();
                await scope.ServiceProvider.GetRequiredService<IFluentEmail>()
                    .To(kycRejectedEvent.EmailId, kycRejectedEvent.FullName)
                    .Subject("Cloud Consult - KYC Rejected")
                    .UsingTemplateFromFile(emailTemplate, kycRejectedEvent)
                    .SendAsync();
                    
                logger.LogInformation($"KYC rejected email sent to {kycRejectedEvent.EmailId}");
            }
            catch (Exception ex)
            {
                logger.LogCritical("Error occured when consuming doctor kyc rejected event");
                logger.LogCritical(ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cluster?.Dispose();
            return Task.CompletedTask;
        }
    }
}