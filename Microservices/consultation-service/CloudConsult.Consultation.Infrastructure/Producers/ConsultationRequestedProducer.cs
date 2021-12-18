using CloudConsult.Common.Kafka;
using CloudConsult.Consultation.Domain.Configurations;
using CloudConsult.Consultation.Domain.Events;
using CloudConsult.Consultation.Domain.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CloudConsult.Consultation.Infrastructure.Producers
{
    [DisallowConcurrentExecution]
    public class ConsultationRequestedProducer : IJob
    {
        private readonly ILogger<ConsultationRequestedProducer> logger;
        private readonly IKafkaProducer<ConsultationRequested> producer;
        private readonly IEventService eventService;
        private readonly QuartzConfiguration config;

        public ConsultationRequestedProducer(ILogger<ConsultationRequestedProducer> logger, IKafkaProducer<ConsultationRequested> producer,
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
                var topicName = config.Jobs.ConsultationRequestedProducer.TopicName;

                var unpublishedEvents = await eventService.GetPendingConsultationRequestedEvents(cancelToken);
                foreach (var unpublishedEvent in unpublishedEvents)
                {
                    var producerTask = producer.ProduceAsync(topicName, unpublishedEvent, cancelToken);

                    await producerTask.ContinueWith(deliveryTask =>
                    {
                        if (deliveryTask.IsFaulted)
                        {
                            logger.LogError($"({unpublishedEvent.ConsultationId}) -> Could not produce consultation requested event message to kafka broker");
                        }
                        else
                        {
                            eventService.SetEventPublished(unpublishedEvent.EventId);
                            logger.LogInformation($"({unpublishedEvent.ConsultationId}) -> Consultation requested event published successfully");
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