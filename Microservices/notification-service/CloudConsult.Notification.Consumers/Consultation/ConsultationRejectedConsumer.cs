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
    public class ConsultationRejectedConsumer : IHostedService
    {
        private readonly ILogger<ConsultationRejectedConsumer> logger;
        private readonly ClusterClient cluster;
        private readonly IServiceProvider serviceProvider;
        private readonly string topicName;
        private readonly string doctorEmailTemplate;
        private readonly string memberEmailTemplate;

        public ConsultationRejectedConsumer(ILogger<ConsultationRejectedConsumer> logger, ClusterClient cluster,
            IServiceProvider serviceProvider, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.serviceProvider = serviceProvider;
            topicName = config["KafkaConfiguration:ConsumerTopics:ConsultationRejectedConsumer"];
            this.doctorEmailTemplate = "Templates/Consultation/ConsultationRejectedDoctorEmail.cshtml";
            this.memberEmailTemplate = "Templates/Consultation/ConsultationRejectedMemberEmail.cshtml";
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

                var consultationRejectedEvent = await JsonSerializer.DeserializeAsync<ConsultationRejected>(reader.BaseStream);
                if (consultationRejectedEvent is null)
                {
                    logger.LogError($"Invalid message format consumed from topic: {topicName}");
                    return;
                }

                using var scope = serviceProvider.CreateScope();
                var emailFactory = scope.ServiceProvider.GetRequiredService<IFluentEmailFactory>();

                //mail to doctor
                await emailFactory.Create()
                    .To(consultationRejectedEvent.DoctorEmailId, consultationRejectedEvent.DoctorName)
                    .Subject("CloudConsult - Consultation Rejected")
                    .UsingTemplateFromFile(doctorEmailTemplate, consultationRejectedEvent)
                    .SendAsync();

                //mail to member
                await emailFactory.Create()
                    .To(consultationRejectedEvent.MemberEmailId, consultationRejectedEvent.MemberName)
                    .Subject("CloudConsult - Consultation Rejected by doctor")
                    .UsingTemplateFromFile(memberEmailTemplate, consultationRejectedEvent)
                    .SendAsync();

                logger.LogInformation($"Consultation Rejected emails sent to {consultationRejectedEvent.DoctorEmailId}, {consultationRejectedEvent.MemberEmailId}");

                //mail to member
            }
            catch (Exception ex)
            {
                logger.LogCritical("Error occured when consuming consultation Rejected event");
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
