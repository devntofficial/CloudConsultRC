using CloudConsult.Notification.Events.Doctor;
using FluentEmail.Core;
using Kafka.Public;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider serviceProvider;
        private readonly string topicName;
        private readonly string emailTemplate;

        public ProfileUpdatedConsumer(ILogger<ProfileUpdatedConsumer> logger, ClusterClient cluster,
            IServiceProvider serviceProvider, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.serviceProvider = serviceProvider;
            topicName = config["KafkaConfiguration:ConsumerTopics:DoctorProfileUpdated"];
            this.emailTemplate = "Templates/Doctor/ProfileUpdatedEmail.cshtml";
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

                var profileUpdatedEvent = await JsonSerializer.DeserializeAsync<ProfileUpdated>(reader.BaseStream);
                if (profileUpdatedEvent is null)
                {
                    logger.LogError($"Invalid message format consumed from topic: {topicName}");
                    return;
                }

                using var scope = serviceProvider.CreateScope();
                await scope.ServiceProvider.GetRequiredService<IFluentEmail>()
                    .To(profileUpdatedEvent.EmailId, profileUpdatedEvent.FullName)
                    .Subject("Cloud Consult - Profile Updated")
                    .UsingTemplateFromFile(emailTemplate, profileUpdatedEvent)
                    .SendAsync();

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
