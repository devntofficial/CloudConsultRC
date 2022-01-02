using AutoMapper;
using CloudConsult.Diagnosis.Domain.Entities;
using CloudConsult.Diagnosis.Domain.Events;
using CloudConsult.Diagnosis.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Diagnosis.Services.MongoDb.Services
{
    public class EventService : IEventService
    {
        private readonly IMongoCollection<DiagnosisReport> reportCollection;
        private readonly IMapper mapper;

        public EventService(IMongoCollection<DiagnosisReport> reportCollection, IMapper mapper)
        {
            this.reportCollection = reportCollection;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ReportUploaded>> GetPendingReportUploadedEvents(CancellationToken cancellationToken = default)
        {
            var events = await reportCollection.Find(x => x.IsEventPublished == false).ToListAsync(cancellationToken);
            return mapper.Map<List<ReportUploaded>>(events);
        }

        public void SetReportUploadedEventPublished(string profileId, bool value, CancellationToken cancellationToken = default)
        {
            var builder = Builders<DiagnosisReport>.Update;
            var update = builder.Set(x => x.IsEventPublished, value);

            reportCollection.UpdateOne(x => x.Id == profileId, update, null, cancellationToken);
        }
    }
}