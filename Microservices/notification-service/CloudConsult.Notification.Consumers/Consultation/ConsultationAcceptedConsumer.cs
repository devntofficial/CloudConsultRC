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
    public class ConsultationAcceptedConsumer : IHostedService
    {
        private readonly ILogger<ConsultationAcceptedConsumer> logger;
        private readonly ClusterClient cluster;
        private readonly IServiceProvider serviceProvider;
        private readonly string topicName;
        private readonly string doctorEmailTemplate;
        private readonly string memberEmailTemplate;

        public ConsultationAcceptedConsumer(ILogger<ConsultationAcceptedConsumer> logger, ClusterClient cluster,
            IServiceProvider serviceProvider, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.serviceProvider = serviceProvider;
            topicName = config["KafkaConfiguration:ConsumerTopics:ConsultationAccepted"];
            this.doctorEmailTemplate = "Templates/Consultation/ConsultationAcceptedDoctorEmail.cshtml";
            this.memberEmailTemplate = "Templates/Consultation/ConsultationAcceptedMemberEmail.cshtml";
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

                var consultationAcceptedEvent = await JsonSerializer.DeserializeAsync<ConsultationAccepted>(reader.BaseStream);
                if (consultationAcceptedEvent is null)
                {
                    logger.LogError($"Invalid message format consumed from topic: {topicName}");
                    return;
                }

                using var scope = serviceProvider.CreateScope();
                var emailFactory = scope.ServiceProvider.GetRequiredService<IFluentEmailFactory>();

                //mail to doctor
                await emailFactory.Create()
                    .To(consultationAcceptedEvent.DoctorEmailId, consultationAcceptedEvent.DoctorName)
                    .Subject("CloudConsult - Consultation Accepted")
                    .UsingTemplateFromFile(doctorEmailTemplate, consultationAcceptedEvent)
                    .SendAsync();

                //mail to member
                await emailFactory.Create()
                    .To(consultationAcceptedEvent.MemberEmailId, consultationAcceptedEvent.MemberName)
                    .Subject("CloudConsult - Consultation accepted by doctor")
                    .UsingTemplateFromFile(memberEmailTemplate, consultationAcceptedEvent)
                    .SendAsync();

                logger.LogInformation($"Consultation Accepted emails sent to {consultationAcceptedEvent.DoctorEmailId}, {consultationAcceptedEvent.MemberEmailId}");

                //mail to member
            }
            catch (Exception ex)
            {
                logger.LogCritical("Error occured when consuming consultation accepted event");
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
