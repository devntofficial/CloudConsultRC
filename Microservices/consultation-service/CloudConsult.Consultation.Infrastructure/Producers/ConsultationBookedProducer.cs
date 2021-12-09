using CloudConsult.Consultation.Domain.Configurations;
using CloudConsult.Consultation.Domain.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace CloudConsult.Consultation.Infrastructure.Producers
{
    public class ConsultationBookedProducer : IJob
    {
        private readonly ILogger<ConsultationBookedProducer> logger;
        private readonly IProducer<Null, string> producer;
        private readonly IEventService eventService;
        private readonly QuartzConfiguration config;

        public ConsultationBookedProducer(ILogger<ConsultationBookedProducer> logger, IProducer<Null, string> producer,
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
                var topicName = config.Jobs.ConsultationBookedProducer.TopicName;

                var unpublishedEvents = await eventService.GetPendingConsultationBookedEvents(cancelToken);
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
                            logger.LogError($"({unpublishedEvent.Id}) -> Could not produce consultation booked event message to kafka broker");
                        }
                        else
                        {
                            eventService.SetConsultationBookedEventPublished(unpublishedEvent.Id);
                            logger.LogInformation($"({unpublishedEvent.Id}) -> Consultation booked event published successfully");
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