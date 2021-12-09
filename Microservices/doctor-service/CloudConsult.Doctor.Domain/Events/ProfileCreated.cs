using CloudConsult.Doctor.Domain.Commands;

namespace CloudConsult.Doctor.Domain.Events
{
    public class ProfileCreated : CreateProfile
    {
        public string ProfileId { get; set; }
    }
}