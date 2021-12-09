using CloudConsult.Common.Email;
using CloudConsult.Notification.Events.Identity;
using Kafka.Public;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace CloudConsult.Notification.Consumers.Identity
{
    public class OtpGeneratedConsumer : IHostedService
    {
        private readonly ILogger<OtpGeneratedConsumer> logger;
        private readonly ClusterClient cluster;
        private readonly SmtpClient emailClient;
        private readonly IEmailService emailService;
        private readonly string topicName;

        public OtpGeneratedConsumer(ILogger<OtpGeneratedConsumer> logger, ClusterClient cluster,
            SmtpClient emailClient, IEmailService emailService, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.emailClient = emailClient;
            this.emailService = emailService;
            topicName = config["KafkaConfiguration:ConsumerTopics:OtpGeneratedConsumer"];
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
                    logger.LogError($"Invalid message payload consumed from topic: {"user-otp-generated"}");
                    return;
                }

                using var memoryStream = new MemoryStream(byteArray);
                using var reader = new StreamReader(memoryStream, Encoding.UTF8);

                var otpEvent = await JsonSerializer.DeserializeAsync<OtpGenerated>(reader.BaseStream);
                if (otpEvent is null)
                {
                    logger.LogError($"Invalid message format consumed from topic: {"user-otp-generated"}");
                    return;
                }

                await emailService.SendTextMail(new EmailServiceTextParameters
                {
                    Subject = "OTP Verification",
                    ToDisplayName = otpEvent.FullName,
                    ToEmail = otpEvent.EmailId,
                    Message = $"Hi {otpEvent.FullName}, Please use {otpEvent.Otp} as a one-time password to verify your account."
                }, emailClient);

                logger.LogInformation($"OTP verification email sent to {otpEvent.EmailId}");
            }
            catch (Exception ex)
            {
                logger.LogCritical("Error occured when consuming otp generated event");
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
