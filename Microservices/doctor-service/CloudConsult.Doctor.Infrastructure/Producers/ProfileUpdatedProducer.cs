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
    public class ProfileUpdatedProducer : IJob
    {
        private readonly ILogger<ProfileUpdatedProducer> _logger;
        private readonly IMapper _mapper;
        private readonly IProducer<Null, string> _producer;
        private readonly IEventService _eventService;
        private readonly QuartzConfiguration _config;

        public ProfileUpdatedProducer(ILogger<ProfileUpdatedProducer> logger,
            IMapper mapper,
            IProducer<Null, string> producer,
            IEventService eventService,
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
                var topicName = _config.Jobs.ProfileUpdatedProducer.TopicName;

                _logger.LogInformation("Looking for any updated doctor profile");

                var unpublishedProfiles = await _eventService.GetUnpublishedUpdatedProfiles(cancelToken);
                
                foreach (var profile in unpublishedProfiles)
                {
                    var mappedEvent = _mapper.Map<ProfileUpdated>(profile);
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
                            await _eventService.SetProfileUpdatedEventPublished(profile.Id, cancelToken);
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
