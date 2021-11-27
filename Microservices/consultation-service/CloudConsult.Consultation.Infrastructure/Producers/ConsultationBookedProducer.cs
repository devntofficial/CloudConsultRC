using System;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using CloudConsult.Consultation.Domain.Configurations;
using CloudConsult.Consultation.Domain.Events;
using CloudConsult.Consultation.Domain.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CloudConsult.Consultation.Infrastructure.Producers
{
    public class ConsultationBookedProducer : IJob
    {
        private readonly ILogger<ConsultationBookedProducer> _logger;
        private readonly IMapper _mapper;
        private readonly IProducer<Null, string> _producer;
        private readonly IConsultationEventService _eventService;
        private readonly QuartzConfiguration _config;
        
        public ConsultationBookedProducer(
            ILogger<ConsultationBookedProducer> logger,
            IMapper mapper,
            IProducer<Null, string> producer,
            IConsultationEventService eventService,
            QuartzConfiguration config)
        {
            _logger = logger;
            _mapper = mapper;
            _producer = producer;
            _eventService = eventService;
            _config = config;
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var cancelToken = context.CancellationToken;
                var topicName = _config.Jobs.ConsultationBookedEventProducer.TopicName;

                _logger.LogInformation("Looking for any new consultations booked recently");

                var consultationEvents = await _eventService.GetUnpublishedBookingEvents(cancelToken);
                foreach (var consultation in consultationEvents)
                {
                    var mappedEvent = _mapper.Map<ConsultationBooked>(consultation);

                    var producerTask = _producer.ProduceAsync(topicName, new Message<Null, string>
                    {
                        Value = JsonSerializer.Serialize(mappedEvent)
                    }, cancelToken);

                    await producerTask.ContinueWith(async deliveryTask =>
                    {
                        if (deliveryTask.IsFaulted)
                        {
                            _logger.LogError("Could not produce message to kafka broker");
                        }
                        else
                        {

                            await _eventService.UpdateBookingEventPublished(consultation.Id, cancelToken);
                            _logger.LogInformation("Event published successfully");
                        }
                    }, cancelToken);
                }
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
            }
        }
    }
}