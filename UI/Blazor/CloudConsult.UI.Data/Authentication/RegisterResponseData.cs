namespace CloudConsult.UI.Data.Authentication
{
    public class RegisterResponseData
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string Roles { get; set; }
        public string AccessToken { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
