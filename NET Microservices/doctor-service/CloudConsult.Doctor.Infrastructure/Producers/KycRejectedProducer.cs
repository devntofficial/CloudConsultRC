using CloudConsult.Common.Kafka;
using CloudConsult.Doctor.Domain.Configurations;
using CloudConsult.Doctor.Domain.Events;
using CloudConsult.Doctor.Domain.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CloudConsult.Doctor.Infrastructure.Producers
{
    [DisallowConcurrentExecution]
    public class KycRejectedProducer : IJob
    {
        private readonly ILogger<KycRejectedProducer> logger;
        private readonly IKafkaProducer<KycRejected> producer;
        private readonly IEventService eventService;
        private readonly QuartzConfiguration config;

        public KycRejectedProducer(ILogger<KycRejectedProducer> logger, IKafkaProducer<KycRejected> producer,
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
                var topicName = config.Jobs.KycRejectedProducer.TopicName;

                var unpublishedEvents = await eventService.GetPendingKycRejectedEvents(cancelToken);

                foreach (var unpublishedEvent in unpublishedEvents)
                {
                    var producerTask = producer.ProduceAsync(topicName, unpublishedEvent, cancelToken);

                    await producerTask.ContinueWith(deliveryTask =>
                    {
                        if (deliveryTask.IsFaulted)
                        {
                            logger.LogError("({ProfileId}) -> Could not produce kyc rejected event message to kafka broker", unpublishedEvent.ProfileId);
                        }
                        else
                        {
                            eventService.SetKycEventPublished(unpublishedEvent.ProfileId);
                            logger.LogInformation("({ProfileId}) -> Kyc rejected event published successfully", unpublishedEvent.ProfileId);
                        }
                    }, cancelToken);
                }
            }
            catch (Exception e)
            {
                logger.LogCritical("{Message}", e.Message);
            }
        }
    }
}
