namespace CloudConsult.Identity.Domain.Configurations
{
    public class QuartzConfiguration
    {
        public int StartDelayInSeconds { get; set; }
        public string SchedulerId { get; set; }
        public string SchedulerName { get; set; }
        public int MisfireThresholdInSeconds { get; set; }
        public int ThreadPoolMaxSize { get; set; }
        public QuartzJobs Jobs { get; set; }
    }

    public class QuartzJobs
    {
        
    }

    public class QuartzJobConfiguration
    {
        public string TopicName { get; set; }
        public string CronExpression { get; set; }
    }
}