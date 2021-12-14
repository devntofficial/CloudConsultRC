using CloudConsult.Member.Domain.Commands;

namespace CloudConsult.Member.Domain.Events
{
    public class ProfileCreated : CreateProfile
    {
        public string ProfileId { get; set; }
        public override string IdentityId { get; set; }
    }
}