using CloudConsult.Common.CQRS;
using CloudConsult.Consultation.Domain.Responses;
using System.Text.Json.Serialization;

namespace CloudConsult.Consultation.Domain.Queries
{
    public class GetTimeSlotsByRange : IQuery<TimeSlotRangeResponse>
    {
        [JsonIgnore]
        public string ProfileId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
