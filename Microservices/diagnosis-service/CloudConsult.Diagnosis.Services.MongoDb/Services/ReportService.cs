using CloudConsult.Diagnosis.Domain.Entities;
using CloudConsult.Diagnosis.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Diagnosis.Services.MongoDb.Services
{
    public class ReportService : IReportService
    {
        private readonly IMongoCollection<DiagnosisReport> reportCollection;

        public ReportService(IMongoCollection<DiagnosisReport> reportCollection)
        {
            this.reportCollection = reportCollection;
        }

        public async Task<DiagnosisReport> GetByConsultationId(string consultationId, CancellationToken cancellationToken = default)
        {
            return await reportCollection.Find(x => x.ConsultationId == consultationId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<DiagnosisReport> GetById(ObjectId reportId, CancellationToken cancellationToken = default)
        {
            return await reportCollection.Find(x => x.Id == reportId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<DiagnosisReport> Upload(DiagnosisReport report, CancellationToken cancellationToken = default)
        {
            report.TimeStamp = DateTime.Now;
            await reportCollection.InsertOneAsync(report, null, cancellationToken);
            return report;
        }
    }
}
