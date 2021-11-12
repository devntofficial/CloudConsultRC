using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudConsult.Doctor.Domain.Events
{
    public class DoctorUpdatedEvent
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
