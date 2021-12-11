﻿using CloudConsult.Doctor.Domain.Configurations;
using CloudConsult.Doctor.Domain.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace CloudConsult.Doctor.Infrastructure.Producers
{
    [DisallowConcurrentExecution]
    public class ProfileUpdatedProducer : IJob
    {
        private readonly ILogger<ProfileUpdatedProducer> logger;
        private readonly IProducer<Null, string> producer;
        private readonly IEventService eventService;
        private readonly QuartzConfiguration config;

        public ProfileUpdatedProducer(ILogger<ProfileUpdatedProducer> logger, IProducer<Null, string> producer,
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
                var topicName = config.Jobs.ProfileUpdatedProducer.TopicName;

                var unpublishedEvents = await eventService.GetPendingProfileUpdatedEvents(cancelToken);

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
                            logger.LogError($"({unpublishedEvent.ProfileId}) -> Could not produce profile updated event message to kafka broker");
                        }
                        else
                        {
                            eventService.SetProfileUpdatedEventPublished(unpublishedEvent.ProfileId);
                            logger.LogInformation($"({unpublishedEvent.ProfileId}) -> Profile updated event published successfully");
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
