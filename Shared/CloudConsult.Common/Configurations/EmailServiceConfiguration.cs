namespace CloudConsult.Common.Configurations
{
    public class EmailServiceConfiguration
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}