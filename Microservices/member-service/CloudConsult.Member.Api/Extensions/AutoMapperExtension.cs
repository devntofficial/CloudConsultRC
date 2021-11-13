using CloudConsult.Common.DependencyInjection;

namespace CloudConsult.Member.Api.Extensions
{
    public class AutoMapperExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(x => {

            });
        }
    }
}
