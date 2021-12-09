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
    public class ProfileUpdatedConsumer : IHostedService
    {
        private readonly ILogger<ProfileUpdatedConsumer> logger;
        private readonly ClusterClient cluster;
        private readonly SmtpClient emailClient;
        private readonly IEmailService emailService;
        private readonly string topicName;

        public ProfileUpdatedConsumer(ILogger<ProfileUpdatedConsumer> logger, ClusterClient cluster,
            SmtpClient emailClient, IEmailService emailService, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.emailClient = emailClient;
            this.emailService = emailService;
            topicName = config["KafkaConfiguration:ConsumerTopics:DoctorProfileUpdatedConsumer"];
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

                var profileUpdatedEvent = await JsonSerializer.DeserializeAsync<ProfileUpdated>(reader.BaseStream);
                if (profileUpdatedEvent is null)
                {
                    logger.LogError($"Invalid message format consumed from topic: {topicName}");
                    return;
                }

                await emailService.SendTextMail(new EmailServiceTextParameters
                {
                    Subject = "Profile updated",
                    ToDisplayName = profileUpdatedEvent.FullName,
                    ToEmail = profileUpdatedEvent.EmailId,
                    Message = $"Hi Dr. {profileUpdatedEvent.FullName}, Your profile was updated recently. If you did not update the profile or you suspect some suspicious activity, please report it to us. Thanks!"
                }, emailClient);

                logger.LogInformation($"Profile updated email sent to {profileUpdatedEvent.EmailId}");
            }
            catch (Exception ex)
            {
                logger.LogCritical("Error occured when consuming doctor profile updated event");
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
