using CloudConsult.Member.Domain.Commands;

namespace CloudConsult.Member.Domain.Events
{
    public class ProfileCreated : CreateProfile
    {
        public string ProfileId { get; set; }
    }
}