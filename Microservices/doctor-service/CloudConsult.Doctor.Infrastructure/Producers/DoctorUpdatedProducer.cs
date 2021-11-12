using System;
using System.Globalization;
using AutoMapper;
using CloudConsult.Doctor.Domain.Events;
using CloudConsult.Doctor.Domain.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using CloudConsult.Doctor.Domain.Configurations;
using Quartz;

namespace CloudConsult.Doctor.Infrastructure.Producers
{
    public class DoctorUpdatedProducer : IJob
    {
        private readonly ILogger<DoctorUpdatedProducer> _logger;
        private readonly IMapper _mapper;
        private readonly IProducer<Null, string> _producer;
        private readonly IDoctorEventService _eventService;
        private readonly QuartzConfiguration _config;

        public DoctorUpdatedProducer(ILogger<DoctorUpdatedProducer> logger,
            IMapper mapper,
            IProducer<Null, string> producer,
            IDoctorEventService eventService,
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
                var topicName = _config.Jobs.DoctorUpdatedProducer.TopicName;

                _logger.LogInformation("Looking for any updated doctor profile");

                var unpublishedDoctors = await _eventService.GetUnpublishedUpdatedDoctors(cancelToken);
                
                foreach (var doctor in unpublishedDoctors)
                {
                    var mappedEvent = _mapper.Map<DoctorUpdatedEvent>(doctor);
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
                            await _eventService.UpdateUpdationEventPublished(doctor.Id, cancelToken);
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
