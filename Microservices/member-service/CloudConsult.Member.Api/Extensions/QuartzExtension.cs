using CloudConsult.Common.DependencyInjection;
using CloudConsult.Member.Domain.Configurations;
using CloudConsult.Member.Infrastructure.Producers;
using Quartz;

namespace CloudConsult.Member.Api.Extensions
{
    public class QuartzExtension : IApiStartupExtension
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var config = new QuartzConfiguration();
            configuration.Bind(nameof(QuartzConfiguration), config);
            services.AddSingleton(config);

            services.AddQuartz(quartz =>
            {
                quartz.SchedulerId = config.SchedulerId;
                quartz.SchedulerName = config.SchedulerName;
                quartz.MisfireThreshold = TimeSpan.FromSeconds(config.MisfireThresholdInSeconds);

                quartz.UseInMemoryStore();
                quartz.UseDefaultThreadPool(opt =>
                {
                    opt.MaxConcurrency = config.ThreadPoolMaxSize;
                });

                quartz.UseMicrosoftDependencyInjectionJobFactory();
                quartz.AddJobAndTrigger<ProfileCreatedProducer>(configuration);
                quartz.AddJobAndTrigger<ProfileUpdatedProducer>(configuration);
            });

            services.AddQuartzHostedService(options =>
            {
                options.StartDelay = TimeSpan.FromSeconds(config.StartDelayInSeconds);
                options.WaitForJobsToComplete = true;
            });
        }
    }
}