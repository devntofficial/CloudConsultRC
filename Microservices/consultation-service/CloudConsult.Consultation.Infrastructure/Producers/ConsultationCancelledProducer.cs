using CloudConsult.Consultation.Domain.Configurations;
using CloudConsult.Consultation.Domain.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace CloudConsult.Consultation.Infrastructure.Producers
{
    [DisallowConcurrentExecution]
    public class ConsultationCancelledProducer : IJob
    {
        private readonly ILogger<ConsultationCancelledProducer> logger;
        private readonly IProducer<Null, string> producer;
        private readonly IEventService eventService;
        private readonly QuartzConfiguration config;

        public ConsultationCancelledProducer(ILogger<ConsultationCancelledProducer> logger, IProducer<Null, string> producer,
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
                var topicName = config.Jobs.ConsultationCancelledProducer.TopicName;

                var unpublishedEvents = await eventService.GetPendingConsultationCancelledEvents(cancelToken);
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
                            logger.LogError($"({unpublishedEvent.ConsultationId}) -> Could not produce consultation cancelled event message to kafka broker");
                        }
                        else
                        {
                            eventService.SetEventPublished(unpublishedEvent.EventId);
                            logger.LogInformation($"({unpublishedEvent.ConsultationId}) -> Consultation cancelled event published successfully");
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