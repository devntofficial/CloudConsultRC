using CloudConsult.Notification.Events.Identity;
using FluentEmail.Core;
using Kafka.Public;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider serviceProvider;
        private readonly string topicName;
        private readonly string emailTemplate;

        public OtpGeneratedConsumer(ILogger<OtpGeneratedConsumer> logger, ClusterClient cluster,
            IServiceProvider serviceProvider, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.serviceProvider = serviceProvider;
            this.topicName = config["KafkaConfiguration:ConsumerTopics:OtpGenerated"];
            this.emailTemplate = "Templates/Identity/OtpGeneratedEmail.cshtml";
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

                using var scope = serviceProvider.CreateScope();
                await scope.ServiceProvider.GetRequiredService<IFluentEmail>()
                    .To(otpEvent.EmailId, otpEvent.FullName)
                    .Subject("Cloud Consult - OTP Verification")
                    .UsingTemplateFromFile(emailTemplate, otpEvent)
                    .SendAsync();
                    
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
