namespace CloudConsult.UI.Data.Authentication
{
    public class LoginResponseData
    {
        public string IdentityId { get; set; }
        public bool IsVerified { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiryTimestamp { get; set; }
    }
}
