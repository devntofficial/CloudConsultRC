using Microsoft.Extensions.Configuration;
using Quartz;

namespace CloudConsult.Common.DependencyInjection
{
    public static class QuartzDI
    {
        public static void AddJobAndTrigger<T>(this IServiceCollectionQuartzConfigurator quartz, IConfiguration config)
        where T : IJob
        {
            var jobName = typeof(T).Name;
            var jobKey = new JobKey(jobName);
            var cronKey = $"QuartzConfiguration:Jobs:{jobName}:CronExpression";
            var cron = config[cronKey];

            if (string.IsNullOrWhiteSpace(cron))
            {
                throw new Exception($"No cron schedule found for job in configuration at {cronKey}");
            }

            quartz.AddJob<T>(options =>
            {
                options.WithIdentity(jobKey);
            });

            quartz.AddTrigger(options =>
            {
                options.ForJob(jobKey);
                options.WithIdentity(jobName + "-Trigger");
                options.WithCronSchedule(cron, x => x.WithMisfireHandlingInstructionDoNothing());
            });
        }
    }
}
