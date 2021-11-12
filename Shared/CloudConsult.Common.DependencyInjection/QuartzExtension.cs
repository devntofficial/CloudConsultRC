using System;
using Microsoft.Extensions.Configuration;
using Quartz;

namespace CloudConsult.Common.DependencyInjection
{
    public static class QuartzExtension
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
            
            quartz.AddJob<T>(opts => opts.WithIdentity(jobKey));
            quartz.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity(jobName + "-Trigger")
                .WithCronSchedule(cron));
        }
    }
}