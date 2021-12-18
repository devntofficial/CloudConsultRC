namespace CloudConsult.UI.Blazor.Models.Identity
{
    public class TokenModel
    {
        public string IdentityId { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiryTimestamp { get; set; }
    }
}
