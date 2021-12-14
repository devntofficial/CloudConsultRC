using CloudConsult.Consultation.Domain.Events;
using CloudConsult.Consultation.Domain.Services;
using Kafka.Public;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace CloudConsult.Consultation.Infrastructure.Consumers
{
    public class PaymentRejectedConsumer : IHostedService
    {
        private readonly ILogger<PaymentRejectedConsumer> logger;
        private readonly ClusterClient cluster;
        private readonly IServiceProvider serviceProvider;
        private readonly string topicName;

        public PaymentRejectedConsumer(ILogger<PaymentRejectedConsumer> logger, ClusterClient cluster,
            IServiceProvider serviceProvider, IConfiguration config)
        {
            this.logger = logger;
            this.cluster = cluster;
            this.serviceProvider = serviceProvider;
            topicName = config["KafkaConfiguration:ConsumerTopics:PaymentRejectedConsumer"];
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            cluster.ConsumeFromLatest(topicName);
            cluster.MessageReceived += async record => await ProcessMessage(record);
            return Task.CompletedTask;
        }

        public async Task ProcessMessage(RawKafkaRecord record)
        {
            try
            {
                if (record is null || record.Value is null)
                {
                    logger.LogError($"Invalid message consumed from topic: {topicName}");
                    return;
                }

                if (record.Value is not byte[] byteArray)
                {
                    logger.LogError($"Invalid message payload consumed from topic: {topicName}");
                    return;
                }

                using var memoryStream = new MemoryStream(byteArray);
                using var reader = new StreamReader(memoryStream, Encoding.UTF8);

                var paymentRejectedEvent = await JsonSerializer.DeserializeAsync<PaymentRejected>(reader.BaseStream);
                if (paymentRejectedEvent is null)
                {
                    logger.LogError($"Invalid message format consumed from topic: {topicName}");
                    return;
                }

                using var scope = serviceProvider.CreateScope();
                var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();
                eventService.SetPaymentRejectedEventConsumed(paymentRejectedEvent.ConsultationId);

                logger.LogInformation("({ConsultationId}) -> Payment rejected message consumed.", paymentRejectedEvent.ConsultationId);
            }
            catch (Exception ex)
            {
                logger.LogCritical("Error occured when consuming payment rejected event");
                logger.LogCritical(ex.Message);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cluster?.Dispose();
            return Task.CompletedTask;
        }
    }
}
