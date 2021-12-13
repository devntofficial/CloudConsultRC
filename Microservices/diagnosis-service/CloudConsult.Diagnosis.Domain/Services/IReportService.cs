using CloudConsult.Diagnosis.Domain.Entities;
using MongoDB.Bson;

namespace CloudConsult.Diagnosis.Domain.Services;

public interface IReportService
{
    Task<DiagnosisReport> Upload(DiagnosisReport report, CancellationToken cancellationToken = default);
    Task<DiagnosisReport> GetById(string reportId, CancellationToken cancellationToken = default);
    Task<DiagnosisReport> GetByConsultationId(string consultationId, CancellationToken cancellationToken = default);
}
