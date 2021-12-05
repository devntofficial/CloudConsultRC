namespace CloudConsult.Identity.Domain.Events;

public class OtpGenerated
{
    public Guid EventId { get; set; }
    public string IdentityId { get; set; }
    public string EmailId { get; set; }
    public string FullName { get; set; }
    public int Otp { get; set; }
}
