using System;

namespace CloudConsult.Identity.Domain.Responses
{
    public class GetTokenResponse
    {
        public string AccessToken { get; set; }
        public DateTime ExpiryTimestamp { get; set; }
    }
}