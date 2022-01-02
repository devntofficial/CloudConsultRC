namespace CloudConsult.Diagnosis.Domain.Configurations
{
    public class MongoDbConfiguration
    {
        public string HostName { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Database { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}