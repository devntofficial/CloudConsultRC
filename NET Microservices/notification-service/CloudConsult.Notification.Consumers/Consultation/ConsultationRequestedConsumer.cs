using CloudConsult.Notification.Events.Consultation;
using FluentEmail.Core;
using Kafka.Public;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace CloudConsult.Notification.Consumers.Consultation
{
    public class ConsultationRequestedConsumer : IHostedService
    {
        private readonly ILogger<ConsultationRequestedConsumer> logger;
        private readonly ClusterClient cluster;
        private readonly IServiceProvider serviceProvider;
        private readonly string topicName;
        private readonly string doctorEmailTemplate;
        private readonly string memberEmailTemplate;

        public ConsultationRequestedConsumer(ILogger<ConsultationRequestedConsumer> logger, ClusterClient cluster,
            IServiceProvider serviceProvider, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.serviceProvider = serviceProvider;
            topicName = config["KafkaConfiguration:ConsumerTopics:ConsultationRequested"];
            this.doctorEmailTemplate = "Templates/Consultation/ConsultationRequestedDoctorEmail.cshtml";
            this.memberEmailTemplate = "Templates/Consultation/ConsultationRequestedMemberEmail.cshtml";
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

                var consultationRequestedEvent = await JsonSerializer.DeserializeAsync<ConsultationRequested>(reader.BaseStream);
                if (consultationRequestedEvent is null)
                {
                    logger.LogError($"Invalid message format consumed from topic: {topicName}");
                    return;
                }

                using var scope = serviceProvider.CreateScope();
                var emailFactory = scope.ServiceProvider.GetRequiredService<IFluentEmailFactory>();

                //mail to doctor
                await emailFactory.Create()
                    .To(consultationRequestedEvent.DoctorEmailId, consultationRequestedEvent.DoctorName) 
                    .Subject("CloudConsult - Consultation request received")
                    .UsingTemplateFromFile(doctorEmailTemplate, consultationRequestedEvent)
                    .SendAsync();

                //mail to member
                await emailFactory.Create()
                    .To(consultationRequestedEvent.MemberEmailId, consultationRequestedEvent.MemberName)
                    .Subject("CloudConsult - Consultation request sent to doctor")
                    .UsingTemplateFromFile(memberEmailTemplate, consultationRequestedEvent)
                    .SendAsync();

                logger.LogInformation($"Consultation requested emails sent to {consultationRequestedEvent.DoctorEmailId}, {consultationRequestedEvent.MemberEmailId}");

                //mail to member
            }
            catch (Exception ex)
            {
                logger.LogCritical("Error occured when consuming doctor kyc approved event");
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
