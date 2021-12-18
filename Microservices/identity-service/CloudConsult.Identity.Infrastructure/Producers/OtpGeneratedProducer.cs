using CloudConsult.Common.Kafka;
using CloudConsult.Identity.Domain.Configurations;
using CloudConsult.Identity.Domain.Events;
using CloudConsult.Identity.Domain.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CloudConsult.Identity.Infrastructure.Producers;

[DisallowConcurrentExecution]
public class OtpGeneratedProducer : IJob
{
    private readonly ILogger<OtpGeneratedProducer> logger;
    private readonly IKafkaProducer<OtpGenerated> producer;
    private readonly IEventService eventService;
    private readonly QuartzConfiguration config;

    public OtpGeneratedProducer(ILogger<OtpGeneratedProducer> logger, IKafkaProducer<OtpGenerated> producer,
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
            var topicName = config.Jobs.OtpGeneratedProducer.TopicName;

            var unpublishedEvents = await eventService.GetPendingOtpGeneratedEvents(cancelToken);
            if (unpublishedEvents is null || unpublishedEvents.Count() == 0) return;

            foreach (var unpublishedEvent in unpublishedEvents)
            {
                var producerTask = producer.ProduceAsync(topicName, unpublishedEvent, cancelToken);

                await producerTask.ContinueWith(deliveryTask =>
                {
                    if (deliveryTask.IsFaulted)
                    {
                        logger.LogError($"({unpublishedEvent.IdentityId}) -> Could not produce otp generated event message to kafka broker");
                    }
                    else
                    {
                        eventService.SetOtpGeneratedEventPublished(unpublishedEvent.EventId, true);
                        logger.LogInformation($"({unpublishedEvent.IdentityId}) -> Otp generated event published successfully");
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
