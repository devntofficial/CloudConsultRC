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
    public class ProfileCreatedConsumer : IHostedService
    {
        private readonly ILogger<ProfileCreatedConsumer> logger;
        private readonly ClusterClient cluster;
        private readonly SmtpClient emailClient;
        private readonly IEmailService emailService;
        private readonly string topicName;

        public ProfileCreatedConsumer(ILogger<ProfileCreatedConsumer> logger, ClusterClient cluster,
            SmtpClient emailClient, IEmailService emailService, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.emailClient = emailClient;
            this.emailService = emailService;
            topicName = config["KafkaConfiguration:ConsumerTopics:DoctorProfileCreatedConsumer"];
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

                var profileCreatedEvent = await JsonSerializer.DeserializeAsync<ProfileCreated>(reader.BaseStream);
                if (profileCreatedEvent is null)
                {
                    logger.LogError($"Invalid message format consumed from topic: {topicName}");
                    return;
                }

                await emailService.SendTextMail(new EmailServiceTextParameters
                {
                    Subject = "Welcome to Cloud Consult",
                    ToDisplayName = profileCreatedEvent.FullName,
                    ToEmail = profileCreatedEvent.EmailId,
                    Message = $"Hi Dr. {profileCreatedEvent.FullName}, Welcome aboard. Your profile was created successfully but you will not be listed on our search till you get your KYC approved by our administration team. Please upload your KYC documents."
                }, emailClient);

                logger.LogInformation($"Welcome email sent to {profileCreatedEvent.EmailId}");
            }
            catch (Exception ex)
            {
                logger.LogCritical("Error occured when consuming doctor profile created event");
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
