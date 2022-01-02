using CloudConsult.Notification.Events.Doctor;
using Kafka.Public;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nest;
using System.Text;
using System.Text.Json;

namespace CloudConsult.Notification.Indexers.Doctor
{
    public class ProfileCreatedIndexer : IHostedService
    {
        private readonly ILogger<ProfileCreatedIndexer> logger;
        private readonly ClusterClient cluster;
        private readonly IElasticClient elasticClient;
        private readonly string topicName;

        public ProfileCreatedIndexer(ILogger<ProfileCreatedIndexer> logger, ClusterClient cluster,
            IConfiguration config, IElasticClient elasticClient)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.elasticClient = elasticClient;
            topicName = config["KafkaConfiguration:ConsumerTopics:DoctorProfileCreated"];
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

                var profileCreatedEvent = await JsonSerializer.DeserializeAsync<ProfileCreated>(reader.BaseStream);
                if (profileCreatedEvent is null)
                {
                    logger.LogError($"Invalid message format consumed from topic: {topicName}");
                    return;
                }

                var indexing = await elasticClient.IndexAsync(profileCreatedEvent, idx => idx.Index("doctorindex"));
                if(indexing.IsValid)
                {
                    logger.LogInformation($"Successfully indexed doctor with profile id: {profileCreatedEvent.ProfileId}");
                }
                else
                {
                    logger.LogError($"Serverside error occured when indexing doctor with profile id: {profileCreatedEvent.ProfileId}");
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical("Error occured when indexing doctor profile created event");
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
