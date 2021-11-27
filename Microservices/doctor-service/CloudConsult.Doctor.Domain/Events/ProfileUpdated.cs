namespace CloudConsult.Doctor.Domain.Events
{
    public class ProfileUpdated
    {
        public string ProfileId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
    }
}
