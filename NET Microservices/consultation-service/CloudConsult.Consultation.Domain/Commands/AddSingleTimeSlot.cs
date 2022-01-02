using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Responses;
using System.Text.Json.Serialization;

namespace CloudConsult.Consultation.Domain.Commands
{
    public class AddSingleTimeSlot : ICommand<TimeSlot>
    {
        [JsonIgnore]
        public string ProfileId { get; set; }
        public DateTime TimeSlotStart { get; set; }
        public DateTime TimeSlotEnd { get; set; }
    }
}
