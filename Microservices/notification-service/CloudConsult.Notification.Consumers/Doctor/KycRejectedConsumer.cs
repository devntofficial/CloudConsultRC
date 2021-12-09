using CloudConsult.Common.Email;
using CloudConsult.Notification.Events.Doctor;
using Kafka.Public;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
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
        private readonly SmtpClient emailClient;
        private readonly IEmailService emailService;
        private readonly string topicName;

        public KycRejectedConsumer(ILogger<KycRejectedConsumer> logger, ClusterClient cluster,
            SmtpClient emailClient, IEmailService emailService, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.emailClient = emailClient;
            this.emailService = emailService;
            topicName = config["KafkaConfiguration:ConsumerTopics:DoctorKycRejectedConsumer"];
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

                var byteArray = record.Value as byte[];
                if (byteArray is null)
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

                await emailService.SendTextMail(new EmailServiceTextParameters
                {
                    Subject = "CloudConsult - KYC Rejected",
                    ToDisplayName = kycRejectedEvent.FullName,
                    ToEmail = kycRejectedEvent.EmailId,
                    Message = $"Hi Dr. {kycRejectedEvent.FullName}, Your KYC documents were rejected by an administrator. Comments: {kycRejectedEvent.Comments}. Please re-upload KYC documents."
                }, emailClient);

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