namespace CloudConsult.UI.Data.Doctor
{
    public class ProfileData
    {
        public string IdentityId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Role { get; set; } = "Doctor";
        public string DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
        public string Speciality { get; set; }
    }
}
