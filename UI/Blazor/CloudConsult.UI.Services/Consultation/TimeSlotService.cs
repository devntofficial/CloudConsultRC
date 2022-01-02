using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Data.Consultation;
using CloudConsult.UI.Interfaces.Consultation;
using CloudConsult.UI.Services.Routes;
using System.Net.Http.Json;
using System.Text.Json;

namespace CloudConsult.UI.Services.Consultation
{
    public class TimeSlotService : ITimeSlotService
    {
        private readonly HttpClient client;
        private readonly JsonSerializerOptions options;

        public TimeSlotService(HttpClient client)
        {
            this.client = client;
            this.options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ApiResponse<TimeSlotResponseData>> AddTimeSlot(string profileId, DateTime timeSlotStart, DateTime timeSlotEnd, CancellationToken cancellationToken = default)
        {
            var url = GatewayRoutes.ConsultationService.AddTimeSlot.Replace("{ProfileId}", profileId);
            var data = new
            {
                TimeSlotStart = timeSlotStart,
                TimeSlotEnd = timeSlotEnd
            };
            var response = await client.PostAsJsonAsync(url, data, cancellationToken);
            return await response.Content.ReadFromJsonAsync<ApiResponse<TimeSlotResponseData>>(options, cancellationToken);
        }

        public async Task<ApiResponse<TimeSlotRangeResponseData>> GetTimeSlots(string profileId, DateTime startDateTime, DateTime endDateTime, CancellationToken cancellationToken = default)
        {
            var url = GatewayRoutes.ConsultationService.GetTimeSlotsRange.Replace("{ProfileId}", profileId);
            var data = new
            {
                ProfileId = profileId,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime
            };
            var response = await client.PostAsJsonAsync(url, data, cancellationToken);
            return await response.Content.ReadFromJsonAsync<ApiResponse<TimeSlotRangeResponseData>>(options, cancellationToken);
        }
    }
}
