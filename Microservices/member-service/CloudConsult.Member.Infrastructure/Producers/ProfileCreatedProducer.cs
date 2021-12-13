using CloudConsult.Member.Domain.Configurations;
using CloudConsult.Member.Domain.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace CloudConsult.Member.Infrastructure.Producers
{
    [DisallowConcurrentExecution]
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
                            logger.LogError("({ProfileId}) -> Could not produce profile created event message to kafka broker", unpublishedEvent.ProfileId);
                        }
                        else
                        {
                            eventService.SetProfileCreatedEventPublished(unpublishedEvent.ProfileId);
                            logger.LogInformation("({ProfileId}) -> Profile created event published successfully", unpublishedEvent.ProfileId);
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