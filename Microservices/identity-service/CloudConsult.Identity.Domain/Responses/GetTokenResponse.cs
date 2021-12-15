using System;

namespace CloudConsult.Identity.Domain.Responses
{
    public class GetTokenResponse
    {
        public string IdentityId { get; set; }
        public bool IsVerified { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiryTimestamp { get; set; }
    }
}