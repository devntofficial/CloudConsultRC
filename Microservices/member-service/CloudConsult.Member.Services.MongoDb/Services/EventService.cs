using CloudConsult.Member.Domain.Entities;
using CloudConsult.Member.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Member.Services.MongoDb.Services
{
    public class EventService : IEventService
    {
        private readonly IMongoCollection<MemberProfile> _profileCollection;

        public EventService(IMongoCollection<MemberProfile> profileCollection)
        {
            this._profileCollection = profileCollection;
        }

        public async Task<IEnumerable<MemberProfile>> GetUnpublishedNewProfiles(CancellationToken cancellationToken = default)
        {
            return await _profileCollection.Find(x => x.ProfileCreatedEventPublished == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<MemberProfile>> GetUnpublishedUpdatedProfiles(CancellationToken cancellationToken = default)
        {
            return await _profileCollection.Find(x => x.ProfileUpdatedEventPublished == false)
                .ToListAsync(cancellationToken);
        }

        public async Task SetProfileUpdatedEventPublished(ObjectId profileId, CancellationToken cancellationToken = default)
        {
            var builder = Builders<MemberProfile>.Update;
            var update = builder.Set(x => x.ProfileUpdatedEventPublished, true);

            await _profileCollection.UpdateOneAsync(x => x.Id == profileId, update, null, cancellationToken);
        }

        public async Task SetProfileCreatedEventPublished(ObjectId profileId, CancellationToken cancellationToken = default)
        {
            var builder = Builders<MemberProfile>.Update;
            var update = builder.Set(x => x.ProfileCreatedEventPublished, true);

            await _profileCollection.UpdateOneAsync(x => x.Id == profileId, update, null, cancellationToken);
        }
    }
}