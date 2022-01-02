namespace CloudConsult.Notification.Events.Identity;

public class OtpGenerated
{
    public string EventId { get; set; } = string.Empty;
    public string IdentityId { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public int Otp { get; set; } = 0;
}