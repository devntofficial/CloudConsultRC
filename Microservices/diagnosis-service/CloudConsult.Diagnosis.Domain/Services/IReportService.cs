using CloudConsult.Diagnosis.Domain.Entities;
using MongoDB.Bson;

namespace CloudConsult.Diagnosis.Domain.Services;

public interface IReportService
{
    Task<DiagnosisReport> Upload(DiagnosisReport report, CancellationToken cancellationToken = default);
    Task<DiagnosisReport> GetById(ObjectId reportId, CancellationToken cancellationToken = default);
    Task<DiagnosisReport> GetByConsultationId(string consultationId, CancellationToken cancellationToken = default);
}
