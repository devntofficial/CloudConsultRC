using CloudConsult.Diagnosis.Domain.Entities;
using MongoDB.Bson;

namespace CloudConsult.Diagnosis.Domain.Services
{
    public interface IEventService
    {
        Task<List<DiagnosisReport>> GetUnpublishedReports(CancellationToken cancellationToken = default);
        Task SetIsEventPublished(ObjectId profileId, bool value, CancellationToken cancellationToken = default);
    }
}
