using CloudConsult.Doctor.Domain.Commands;

namespace CloudConsult.Doctor.Domain.Events
{
    public class ProfileUpdated : UpdateProfile
    {
        //extra fields specific to the event can come here in the future
        public override string ProfileId { get; set; }
        public override string IdentityId { get; set; }
    }
}
