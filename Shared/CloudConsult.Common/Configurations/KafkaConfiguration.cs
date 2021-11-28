namespace CloudConsult.Common.Configurations;

public class KafkaConfiguration
{
    public string BootstrapServers { get; set; }
    public string Acks { get; set; }
    public int MessageSendMaxRetries { get; set; }
    public int RetryBackoffMs { get; set; }
    public int MessageTimeoutMs { get; set; }
    public bool EnableIdempotence { get; set; }
}