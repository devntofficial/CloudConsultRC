using CloudConsult.Common.Kafka;
using CloudConsult.Member.Domain.Configurations;
using CloudConsult.Member.Domain.Events;
using CloudConsult.Member.Domain.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CloudConsult.Member.Infrastructure.Producers
{
    [DisallowConcurrentExecution]
    public class ProfileCreatedProducer : IJob
    {
        private readonly ILogger<ProfileCreatedProducer> logger;
        private readonly IKafkaProducer<ProfileCreated> producer;
        private readonly IEventService eventService;
        private readonly QuartzConfiguration config;

        public ProfileCreatedProducer(ILogger<ProfileCreatedProducer> logger, IKafkaProducer<ProfileCreated> producer,
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
                    var producerTask = producer.ProduceAsync(topicName, unpublishedEvent, cancelToken);

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