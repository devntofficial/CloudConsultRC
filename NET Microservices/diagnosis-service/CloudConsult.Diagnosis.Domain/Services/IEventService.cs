using CloudConsult.Diagnosis.Domain.Entities;
using CloudConsult.Diagnosis.Domain.Events;
using MongoDB.Bson;

namespace CloudConsult.Diagnosis.Domain.Services
{
    public interface IEventService
    {
        Task<IEnumerable<ReportUploaded>> GetPendingReportUploadedEvents(CancellationToken cancellationToken = default);
        void SetReportUploadedEventPublished(string profileId, bool value, CancellationToken cancellationToken = default);
    }
}
