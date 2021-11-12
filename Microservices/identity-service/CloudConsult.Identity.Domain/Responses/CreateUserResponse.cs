using System;

namespace CloudConsult.Identity.Domain.Responses
{
    public class CreateUserResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string EmailId { get; set; }
        public string Roles { get; set; }
        public DateTime Timestamp { get; set; }
    }
}