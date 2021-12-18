namespace CloudConsult.UI.Blazor.Models.Doctor
{
    public class ProfileModel
    {
        public string ProfileId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Role { get; set; } = "Doctor";
        public DateTime? DateOfBirth { get; set; }
        public string? MobileNo { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string AadhaarNo { get; set; } = string.Empty;
        public string Speciality { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
    }
}
