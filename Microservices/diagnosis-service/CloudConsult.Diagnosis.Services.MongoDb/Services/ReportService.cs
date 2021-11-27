using CloudConsult.Diagnosis.Domain.Entities;
using CloudConsult.Diagnosis.Domain.Services;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudConsult.Diagnosis.Services.MongoDb.Services
{
    public class ReportService : IReportService
    {
        private readonly IMongoCollection<DiagnosisReport> reportCollection;

        public ReportService(IMongoCollection<DiagnosisReport> reportCollection)
        {
            this.reportCollection = reportCollection;
        }

        public async Task<DiagnosisReport> Upload(DiagnosisReport report, CancellationToken cancellationToken = default)
        {
            report.TimeStamp = DateTime.UtcNow;
            await reportCollection.InsertOneAsync(report, null, cancellationToken);
            return report;
        }
    }
}
