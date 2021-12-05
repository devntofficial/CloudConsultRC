using CloudConsult.Diagnosis.Domain.Entities;
using CloudConsult.Diagnosis.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Diagnosis.Services.MongoDb.Services
{
    public class EventService : IEventService
    {
        private readonly IMongoCollection<DiagnosisReport> reportCollection;

        public EventService(IMongoCollection<DiagnosisReport> reportCollection)
        {
            this.reportCollection = reportCollection;
        }

        public async Task<List<DiagnosisReport>> GetUnpublishedReports(CancellationToken cancellationToken = default)
        {
            return await reportCollection.Find(x => x.IsEventPublished == false).ToListAsync(cancellationToken);
        }

        public void SetIsEventPublished(ObjectId profileId, bool value, CancellationToken cancellationToken = default)
        {
            var builder = Builders<DiagnosisReport>.Update;
            var update = builder.Set(x => x.IsEventPublished, value);

            reportCollection.UpdateOne(x => x.Id == profileId, update, null, cancellationToken);
        }
    }
}