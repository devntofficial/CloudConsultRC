using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Entities;
using CloudConsult.Consultation.Domain.Responses;

namespace CloudConsult.Consultation.Domain.Services;

public interface ITimeSlotService
{
    Task AddDoctorTimeSlots(IEnumerable<DoctorTimeSlot> availabilities, CancellationToken cancellationToken = default);
    Task<(TimeSlot response, bool isSuccess, string message)> AddSingleTimeSlot(AddSingleTimeSlot command, CancellationToken cancellationToken = default);

    Task<IEnumerable<DoctorTimeSlot>> GetDoctorTimeSlots(string doctorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DoctorTimeSlot>> GetDoctorTimeSlotsRange(string doctorId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}