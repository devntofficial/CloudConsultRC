using CloudConsult.UI.Data.Common;
using CloudConsult.UI.Data.Consultation;

namespace CloudConsult.UI.Interfaces.Consultation
{
    public interface ITimeSlotService
    {
        Task<ApiResponse<TimeSlotRangeResponseData>> GetTimeSlots(string profileId, DateTime startDateTime, DateTime endDateTime, CancellationToken cancellationToken = default);
    }
}
