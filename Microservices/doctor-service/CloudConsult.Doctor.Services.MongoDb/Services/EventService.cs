using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Doctor.Services.MongoDb.Services
{
    public class EventService : IEventService
    {
        private readonly IMongoCollection<DoctorProfile> _profileCollection;

        public EventService(IMongoCollection<DoctorProfile> profileCollection)
        {
            this._profileCollection = profileCollection;
        }

        public async Task<IEnumerable<DoctorProfile>> GetUnpublishedNewProfiles(CancellationToken cancellationToken = default)
        {
            return await _profileCollection.Find(x => x.ProfileCreatedEventPublished == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<DoctorProfile>> GetUnpublishedUpdatedProfiles(CancellationToken cancellationToken = default)
        {
            return await _profileCollection.Find(x => x.ProfileUpdatedEventPublished == false)
                .ToListAsync(cancellationToken);
        }

        public async Task SetProfileUpdatedEventPublished(ObjectId profileId, CancellationToken cancellationToken = default)
        {
            var builder = Builders<DoctorProfile>.Update;
            var update = builder.Set(x => x.ProfileUpdatedEventPublished, true);

            await _profileCollection.UpdateOneAsync(x => x.Id == profileId, update, null, cancellationToken);
        }

        public async Task SetProfileCreatedEventPublished(ObjectId profileId, CancellationToken cancellationToken = default)
        {
            var builder = Builders<DoctorProfile>.Update;
            var update = builder.Set(x => x.ProfileCreatedEventPublished, true);

            await _profileCollection.UpdateOneAsync(x => x.Id == profileId, update, null, cancellationToken);
        }
    }
}