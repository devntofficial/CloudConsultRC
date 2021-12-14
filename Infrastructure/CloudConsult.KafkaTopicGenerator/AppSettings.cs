namespace CloudConsult.KafkaTopicGenerator
{
    internal class AppSettings
    {
        public string BootstrapServers { get; set; } = string.Empty;
        public List<KafkaTopic> KafkaTopics { get; set; } = new();
    }

    internal class KafkaTopic
    {
        public string Name { get; set; } = string.Empty;
        public short ReplicationFactor { get; set; } = 1;
        public int PartitionCount { get; set; } = 1;
    }
}
