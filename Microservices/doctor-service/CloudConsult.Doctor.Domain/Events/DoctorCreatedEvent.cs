using CloudConsult.Doctor.Domain.Commands;

namespace CloudConsult.Doctor.Domain.Events
{
    public class DoctorCreatedEvent
    {
        public string DoctorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string AadhaarNo { get; set; }
    }
}