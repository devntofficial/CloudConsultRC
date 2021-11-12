using System;

namespace CloudConsult.UI.Models
{
    public class AuthenticatedUserModel
    {
        public string AccessToken { get; set; }
        public DateTime ExpiryTimestamp { get; set; }
    }
}
