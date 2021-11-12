using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudConsult.Doctor.Domain.Entities;
using MongoDB.Bson;

namespace CloudConsult.Doctor.Domain.Services
{
    public interface IDoctorEventService
    {
        Task<IEnumerable<DoctorEntity>> GetUnpublishedCreatedDoctors(CancellationToken cancellationToken = default);
        Task<IEnumerable<DoctorEntity>> GetUnpublishedUpdatedDoctors(CancellationToken cancellationToken = default);
        Task UpdateRegistrationEventPublished(ObjectId doctorId, CancellationToken cancellationToken = default);
        Task UpdateUpdationEventPublished(ObjectId doctorId, CancellationToken cancellationToken = default);
    }
}