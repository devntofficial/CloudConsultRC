using CloudConsult.Common.Configurations;
using CloudConsult.Common.Kafka;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudConsult.Common.DependencyInjection
{
    public static class KafkaDI
    {
        public static IServiceCollection AddCommonKafkaProducer(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new KafkaConfiguration();
            configuration.Bind(nameof(KafkaConfiguration), config);
            services.AddSingleton(config);

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = config.BootstrapServers,
                Acks = config.Acks == "All" ? Acks.All : config.Acks == "Leader" ? Acks.Leader : Acks.None,
                MessageSendMaxRetries = config.MessageSendMaxRetries,
                MessageTimeoutMs = config.MessageTimeoutMs,
                RetryBackoffMs = config.RetryBackoffMs,
                EnableIdempotence = config.EnableIdempotence
            };
            services.AddSingleton(producerConfig);
            services.AddSingleton(typeof(IKafkaProducer<>), typeof(KafkaProducer<>));
            return services;
        }
    }
}
