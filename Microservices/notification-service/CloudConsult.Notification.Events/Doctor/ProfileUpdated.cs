namespace CloudConsult.Notification.Events.Doctor;

public class ProfileUpdated
{
    public string ProfileId { get; set; } = string.Empty;
    public string IdentityId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = string.Empty;
    public string MobileNo { get; set; } = string.Empty;
    public string EmailId { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string AadhaarNo { get; set; } = string.Empty;
    public string Speciality { get; set; } = string.Empty;
}