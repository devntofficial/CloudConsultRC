
namespace CloudConsult.Common.Email
{
    public class EmailServiceTextParameters
    {
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string FromDisplayName { get; set; }
        public string ToDisplayName { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}