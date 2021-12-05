using CloudConsult.Doctor.Domain.Configurations;
using CloudConsult.Doctor.Domain.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace CloudConsult.Doctor.Infrastructure.Producers
{
    public class ProfileCreatedProducer : IJob
    {
        private readonly ILogger<ProfileCreatedProducer> logger;
        private readonly IProducer<Null, string> producer;
        private readonly IEventService eventService;
        private readonly QuartzConfiguration config;

        public ProfileCreatedProducer(ILogger<ProfileCreatedProducer> logger, IProducer<Null, string> producer,
            IEventService eventService, QuartzConfiguration config)
        {
            this.logger = logger;
            this.producer = producer;
            this.eventService = eventService;
            this.config = config;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var cancelToken = context.CancellationToken;
                var topicName = config.Jobs.ProfileCreatedProducer.TopicName;

                var unpublishedEvents = await eventService.GetPendingProfileCreatedEvents(cancelToken);
                foreach (var unpublishedEvent in unpublishedEvents)
                {
                    var producerTask = producer.ProduceAsync(topicName, new Message<Null, string>
                    {
                        Value = JsonSerializer.Serialize(unpublishedEvent)
                    }, cancelToken);

                    await producerTask.ContinueWith(deliveryTask =>
                    {
                        if (deliveryTask.IsFaulted)
                        {
                            logger.LogError($"({unpublishedEvent.ProfileId}) -> Could not produce profile created event message to kafka broker");
                        }
                        else
                        {
                            eventService.SetProfileCreatedEventPublished(unpublishedEvent.ProfileId, cancelToken);
                            logger.LogInformation($"({unpublishedEvent.ProfileId}) -> Profile created event published successfully");
                        }
                    }, cancelToken);
                }
            }
            catch (Exception e)
            {
                logger.LogCritical(e.Message);
            }
        }
    }
}