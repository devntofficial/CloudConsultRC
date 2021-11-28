using System;

namespace CloudConsult.UI.Models.Identity
{
    public class AuthenticatedUserModel
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiryTimestamp { get; set; }
    }
}
