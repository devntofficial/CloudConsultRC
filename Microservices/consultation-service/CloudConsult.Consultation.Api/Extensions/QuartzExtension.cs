using CloudConsult.Common.DependencyInjection;
using CloudConsult.Consultation.Domain.Configurations;
using CloudConsult.Consultation.Infrastructure.Producers;
using Quartz;

namespace CloudConsult.Consultation.Api.Extensions;

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
            quartz.AddJobAndTrigger<ConsultationBookedProducer>(configuration);
        });

        services.AddQuartzHostedService(options =>
        {
            options.StartDelay = TimeSpan.FromSeconds(config.StartDelayInSeconds);
            options.WaitForJobsToComplete = true;
        });
    }
}