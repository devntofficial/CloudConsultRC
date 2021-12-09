namespace CloudConsult.Doctor.Domain.Events;
public class KycRejected
{
    public string ProfileId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string AdministratorId { get; set; } = string.Empty;
    public string Comments { get; set; } = string.Empty;
}
