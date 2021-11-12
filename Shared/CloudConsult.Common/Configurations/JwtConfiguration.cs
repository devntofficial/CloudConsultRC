namespace CloudConsult.Common.Configurations
{
    public class JwtConfiguration
    {
        public string SecretKey { get; set; }
        public int ExpiryTimeInMinutes { get; set; }
    }
}