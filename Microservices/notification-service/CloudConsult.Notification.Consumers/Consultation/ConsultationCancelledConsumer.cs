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
    public class ConsultationCancelledConsumer : IHostedService
    {
        private readonly ILogger<ConsultationCancelledConsumer> logger;
        private readonly ClusterClient cluster;
        private readonly IServiceProvider serviceProvider;
        private readonly string topicName;
        private readonly string doctorEmailTemplate;
        private readonly string memberEmailTemplate;

        public ConsultationCancelledConsumer(ILogger<ConsultationCancelledConsumer> logger, ClusterClient cluster,
            IServiceProvider serviceProvider, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.serviceProvider = serviceProvider;
            topicName = config["KafkaConfiguration:ConsumerTopics:ConsultationCancelledConsumer"];
            this.doctorEmailTemplate = "Templates/Consultation/ConsultationCancelledDoctorEmail.cshtml";
            this.memberEmailTemplate = "Templates/Consultation/ConsultationCancelledMemberEmail.cshtml";
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

                var consultationCancelledEvent = await JsonSerializer.DeserializeAsync<ConsultationCancelled>(reader.BaseStream);
                if (consultationCancelledEvent is null)
                {
                    logger.LogError($"Invalid message format consumed from topic: {topicName}");
                    return;
                }

                using var scope = serviceProvider.CreateScope();
                var emailFactory = scope.ServiceProvider.GetRequiredService<IFluentEmailFactory>();

                //mail to doctor
                await emailFactory.Create()
                    .To(consultationCancelledEvent.DoctorEmailId, consultationCancelledEvent.DoctorName)
                    .Subject("CloudConsult - Consultation Cancelled")
                    .UsingTemplateFromFile(doctorEmailTemplate, consultationCancelledEvent)
                    .SendAsync();

                //mail to member
                await emailFactory.Create()
                    .To(consultationCancelledEvent.MemberEmailId, consultationCancelledEvent.MemberName)
                    .Subject("CloudConsult - Consultation Cancelled by doctor")
                    .UsingTemplateFromFile(memberEmailTemplate, consultationCancelledEvent)
                    .SendAsync();

                logger.LogInformation($"Consultation Cancelled emails sent to {consultationCancelledEvent.DoctorEmailId}, {consultationCancelledEvent.MemberEmailId}");

                //mail to member
            }
            catch (Exception ex)
            {
                logger.LogCritical("Error occured when consuming consultation Cancelled event");
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
