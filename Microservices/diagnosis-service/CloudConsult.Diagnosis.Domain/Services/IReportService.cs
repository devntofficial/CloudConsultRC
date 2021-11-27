using CloudConsult.Diagnosis.Domain.Entities;

namespace CloudConsult.Diagnosis.Domain.Services;

public interface IReportService
{
    Task<DiagnosisReport> Upload(DiagnosisReport report, CancellationToken cancellationToken = default);
}
