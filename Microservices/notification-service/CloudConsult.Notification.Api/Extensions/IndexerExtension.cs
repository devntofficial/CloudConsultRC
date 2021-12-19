using CloudConsult.Common.DependencyInjection;
using CloudConsult.Notification.Indexers.Doctor;
using Elasticsearch.Net;
using Nest;

namespace CloudConsult.Notification.Api.Extensions
{
    public class IndexerExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var nodes = configuration["ElasticsearchServers"]
                .Split(',')
                .Select(x => new Uri(x));

            //will add cluster pooling later
            var pool = new StaticConnectionPool(nodes);
            var settings = new ConnectionSettings(pool);

            services.AddSingleton<IElasticClient>(x => new ElasticClient(settings));

            services.AddHostedService<ProfileCreatedIndexer>();
            services.AddHostedService<ProfileUpdatedIndexer>();
        }
    }
}
