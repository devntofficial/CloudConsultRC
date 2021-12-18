using CloudConsult.Common.DependencyInjection;
using Nest;

namespace CloudConsult.Notification.Api.Extensions
{
    public class IndexerExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //var elasticSearchServer = new Uri(configuration["ElasticsearchServer"]);
            //services.AddSingleton<IElasticClient>(x => new ElasticClient(new ConnectionSettings(elasticSearchServer)));


        }
    }
}
