namespace CloudConsult.Diagnosis.Domain.Configurations
{
    public class QuartzConfiguration
    {
        public int StartDelayInSeconds { get; set; }
        public string SchedulerId { get; set; } = string.Empty;
        public string SchedulerName { get; set; } = string.Empty;
        public int MisfireThresholdInSeconds { get; set; }
        public int ThreadPoolMaxSize { get; set; }
        public QuartzJobs Jobs { get; set; } = new();
    }

    public class QuartzJobs
    {
        public QuartzJobConfiguration ReportUploadedProducer { get; set; } = new();
    }

    public class QuartzJobConfiguration
    {
        public string TopicName { get; set; } = string.Empty;
        public string CronExpression { get; set; } = string.Empty;
    }
}