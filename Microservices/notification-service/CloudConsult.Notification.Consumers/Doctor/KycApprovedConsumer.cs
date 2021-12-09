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
    public class KycApprovedConsumer : IHostedService
    {
        private readonly ILogger<KycApprovedConsumer> logger;
        private readonly ClusterClient cluster;
        private readonly SmtpClient emailClient;
        private readonly IEmailService emailService;
        private readonly string topicName;

        public KycApprovedConsumer(ILogger<KycApprovedConsumer> logger, ClusterClient cluster,
            SmtpClient emailClient, IEmailService emailService, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.emailClient = emailClient;
            this.emailService = emailService;
            topicName = config["KafkaConfiguration:ConsumerTopics:DoctorKycApprovedConsumer"];
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

                var kycApprovedEvent = await JsonSerializer.DeserializeAsync<KycApproved>(reader.BaseStream);
                if (kycApprovedEvent is null)
                {
                    logger.LogError($"Invalid message format consumed from topic: {topicName}");
                    return;
                }

                await emailService.SendTextMail(new EmailServiceTextParameters
                {
                    Subject = "CloudConsult - KYC approved",
                    ToDisplayName = kycApprovedEvent.FullName,
                    ToEmail = kycApprovedEvent.EmailId,
                    Message = $"Hi Dr. {kycApprovedEvent.FullName}, Your KYC documents were successfully verified and approved by an administrator. Your profile will now show up in search results."
                }, emailClient);

                logger.LogInformation($"KYC Approved email sent to {kycApprovedEvent.EmailId}");
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