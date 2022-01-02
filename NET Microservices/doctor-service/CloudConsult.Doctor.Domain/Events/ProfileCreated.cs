using CloudConsult.Doctor.Domain.Commands;

namespace CloudConsult.Doctor.Domain.Events
{
    public class ProfileCreated : CreateProfile
    {
        public string ProfileId { get; set; }
        public override string IdentityId { get; set; }
    }
}