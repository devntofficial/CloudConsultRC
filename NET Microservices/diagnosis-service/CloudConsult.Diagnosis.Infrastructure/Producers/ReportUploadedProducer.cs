using CloudConsult.Common.Kafka;
using CloudConsult.Diagnosis.Domain.Configurations;
using CloudConsult.Diagnosis.Domain.Events;
using CloudConsult.Diagnosis.Domain.Services;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CloudConsult.Diagnosis.Infrastructure.Producers
{
    [DisallowConcurrentExecution]
    public class ReportUploadedProducer : IJob
    {
        private readonly ILogger<ReportUploadedProducer> logger;
        private readonly IKafkaProducer<ReportUploaded> producer;
        private readonly IEventService eventService;
        private readonly QuartzConfiguration config;

        public ReportUploadedProducer(ILogger<ReportUploadedProducer> logger, IKafkaProducer<ReportUploaded> producer,
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
                var topicName = config.Jobs.ReportUploadedProducer.TopicName;

                var unpublishedReports = await eventService.GetPendingReportUploadedEvents(cancelToken);
                foreach (var report in unpublishedReports)
                {
                    try
                    {
                        var producerTask = producer.ProduceAsync(topicName, report, cancelToken);

                        await producerTask.ContinueWith(deliveryTask =>
                        {
                            if (deliveryTask.IsFaulted)
                            {
                                logger.LogError("Could not produce message to kafka broker");
                            }
                            else
                            {
                                eventService.SetReportUploadedEventPublished(report.ReportId, true, cancelToken);
                                logger.LogInformation("({ReportId}) -> Diagnosis report uploaded event published successfully", report.ReportId);
                            }
                        }, cancelToken);
                    }
                    catch
                    {
                        logger.LogCritical("({ReportId}) -> Error in publishing diagnosis report uploaded event", report.ReportId);
                        continue;
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogCritical("{Message}", e.Message);
            }
        }
    }
}
