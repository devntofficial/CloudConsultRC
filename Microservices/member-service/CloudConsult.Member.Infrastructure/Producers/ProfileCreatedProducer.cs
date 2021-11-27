using AutoMapper;
using CloudConsult.Member.Domain.Configurations;
using CloudConsult.Member.Domain.Events;
using CloudConsult.Member.Domain.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace CloudConsult.Member.Infrastructure.Producers
{
    public class ProfileCreatedProducer : IJob
    {
        private readonly ILogger<ProfileCreatedProducer> _logger;
        private readonly IMapper _mapper;
        private readonly IProducer<Null, string> _producer;
        private readonly IEventService _eventService;
        private readonly QuartzConfiguration _config;

        public ProfileCreatedProducer(
            ILogger<ProfileCreatedProducer> logger,
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
                var topicName = _config.Jobs.ProfileCreatedProducer.TopicName;

                _logger.LogInformation("Looking for any new member profile created recently");

                var unpublishedProfiles = await _eventService.GetUnpublishedNewProfiles(cancelToken);
                foreach (var profile in unpublishedProfiles)
                {
                    var mappedEvent = _mapper.Map<ProfileCreated>(profile);

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
                            await _eventService.SetProfileCreatedEventPublished(profile.Id, cancelToken);
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