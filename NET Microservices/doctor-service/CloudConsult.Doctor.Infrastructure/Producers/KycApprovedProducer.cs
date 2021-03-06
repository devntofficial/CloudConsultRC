using CloudConsult.Common.Kafka;
using CloudConsult.Doctor.Domain.Configurations;
using CloudConsult.Doctor.Domain.Events;
using CloudConsult.Doctor.Domain.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CloudConsult.Doctor.Infrastructure.Producers
{
    [DisallowConcurrentExecution]
    public class KycApprovedProducer : IJob
    {
        private readonly ILogger<KycApprovedProducer> logger;
        private readonly IKafkaProducer<KycApproved> producer;
        private readonly IEventService eventService;
        private readonly QuartzConfiguration config;

        public KycApprovedProducer(ILogger<KycApprovedProducer> logger, IKafkaProducer<KycApproved> producer,
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
                var topicName = config.Jobs.KycApprovedProducer.TopicName;

                var unpublishedEvents = await eventService.GetPendingKycApprovedEvents(cancelToken);

                foreach (var unpublishedEvent in unpublishedEvents)
                {
                    var producerTask = producer.ProduceAsync(topicName, unpublishedEvent, cancelToken);

                    await producerTask.ContinueWith(deliveryTask =>
                    {
                        if (deliveryTask.IsFaulted)
                        {
                            logger.LogError($"({unpublishedEvent.ProfileId}) -> Could not produce kyc approved event message to kafka broker");
                        }
                        else
                        {
                            eventService.SetKycEventPublished(unpublishedEvent.ProfileId);
                            logger.LogInformation($"({unpublishedEvent.ProfileId}) -> Kyc approved event published successfully");
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
