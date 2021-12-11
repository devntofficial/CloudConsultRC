using AutoMapper;
using CloudConsult.Diagnosis.Domain.Configurations;
using CloudConsult.Diagnosis.Domain.Events;
using CloudConsult.Diagnosis.Domain.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json;

namespace CloudConsult.Diagnosis.Infrastructure.Producers
{
    [DisallowConcurrentExecution]
    public class ReportUploadedProducer : IJob
    {
        private readonly ILogger<ReportUploadedProducer> logger;
        private readonly IMapper mapper;
        private readonly IProducer<Null, string> producer;
        private readonly IEventService eventService;
        private readonly QuartzConfiguration config;

        public ReportUploadedProducer(ILogger<ReportUploadedProducer> logger, IMapper mapper,
            IProducer<Null, string> producer, IEventService eventService, QuartzConfiguration config)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.producer = producer;
            this.eventService = eventService;
            this.config = config;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                var cancelToken = context.CancellationToken;
                var topicName = config.Jobs.ReportUploadedProducer.TopicName;

                logger.LogInformation("Looking for recently uploaded diagnosis reports");

                var unpublishedReports = await eventService.GetUnpublishedReports(cancelToken);
                foreach (var report in unpublishedReports)
                {
                    try
                    {
                        var mappedEvent = mapper.Map<ReportUploaded>(report);

                        var producerTask = producer.ProduceAsync(topicName, new Message<Null, string>
                        {
                            Value = JsonSerializer.Serialize(mappedEvent)
                        }, cancelToken);

                        await producerTask.ContinueWith(deliveryTask =>
                        {
                            if (deliveryTask.IsFaulted)
                            {
                                logger.LogError("Could not produce message to kafka broker");
                            }
                            else
                            {
                                eventService.SetIsEventPublished(report.Id, true, cancelToken);
                                logger.LogInformation($"Diagnosis report uploaded event published successfully for {report.Id}");
                            }
                        }, cancelToken);
                    }
                    catch (Exception ex)
                    {
                        logger.LogCritical($"Diagnosis report uploaded event published is failing for {report.Id} with message: {ex.Message}");
                        continue;
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogCritical(e.Message);
            }
        }
    }
}
