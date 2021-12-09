namespace CloudConsult.Doctor.Domain.Responses
{
    public record ProfileResponse
    {
        public string ProfileId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
        public string Speciality { get; set; }
        public bool IsActive { get; set; }
    }
}