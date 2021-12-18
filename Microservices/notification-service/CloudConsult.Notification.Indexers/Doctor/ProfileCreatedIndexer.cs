using Microsoft.Extensions.Hosting;
using Nest;

namespace CloudConsult.Notification.Indexers.Doctor
{
    public class ProfileCreatedIndexer : BackgroundService
    {
        private readonly IElasticClient elasticClient;

        public ProfileCreatedIndexer(IElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }
    }
}
