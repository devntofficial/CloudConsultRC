using CloudConsult.Common.Kafka;
using CloudConsult.Doctor.Domain.Configurations;
using CloudConsult.Doctor.Domain.Events;
using CloudConsult.Doctor.Domain.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CloudConsult.Doctor.Infrastructure.Producers
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
                var topicName = config.Jobs.ProfileCreatedProducer.TopicName;
                var pendingEvents = await eventService.GetPendingProfileCreatedEvents(context.CancellationToken);
                var pendingTasks = pendingEvents.Select(pendingEvent =>
                {
                    var delivery = producer.ProduceAsync(topicName, pendingEvent, context.CancellationToken);

                    if (delivery.IsFaulted)
                    {
                        logger.LogError($"({pendingEvent.ProfileId}) -> Could not produce profile created event message to kafka broker");
                    }
                    else
                    {
                        eventService.SetProfileCreatedEventPublished(pendingEvent.ProfileId);
                        logger.LogInformation($"({pendingEvent.ProfileId}) -> Profile created event published successfully");
                    }
                    return delivery;
                }).ToArray();

                await Task.WhenAll(pendingTasks);
            }
            catch (Exception e)
            {
                logger.LogCritical(e.Message);
            }
        }
    }
}