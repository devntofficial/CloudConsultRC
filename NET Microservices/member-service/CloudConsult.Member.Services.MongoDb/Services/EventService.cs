using AutoMapper;
using CloudConsult.Member.Domain.Entities;
using CloudConsult.Member.Domain.Events;
using CloudConsult.Member.Domain.Services;
using MongoDB.Driver;

namespace CloudConsult.Member.Services.MongoDb.Services
{
    public class EventService : IEventService
    {
        private readonly IMongoCollection<MemberProfile> profileCollection;
        private readonly IMapper mapper;

        public EventService(IMongoCollection<MemberProfile> profileCollection, IMapper mapper)
        {
            this.profileCollection = profileCollection;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<ProfileCreated>> GetPendingProfileCreatedEvents(CancellationToken cancellationToken = default)
        {
            var profiles = await profileCollection.Find(x => !x.IsCreatedEventPublished).ToListAsync(cancellationToken);
            return profiles.Count > 0 ? mapper.Map<List<ProfileCreated>>(profiles) : Enumerable.Empty<ProfileCreated>();
        }

        public async Task<IEnumerable<ProfileUpdated>> GetPendingProfileUpdatedEvents(CancellationToken cancellationToken = default)
        {
            var profiles = await profileCollection.Find(x => !x.IsUpdatedEventPublished).ToListAsync(cancellationToken);
            return profiles.Count > 0 ? mapper.Map<List<ProfileUpdated>>(profiles) : Enumerable.Empty<ProfileUpdated>();
        }

        public void SetProfileUpdatedEventPublished(string profileId)
        {
            var builder = Builders<MemberProfile>.Update;
            var update = builder.Set(x => x.IsUpdatedEventPublished, true);

            profileCollection.UpdateOne(x => x.Id == profileId, update, null);
        }

        public void SetProfileCreatedEventPublished(string profileId)
        {
            var builder = Builders<MemberProfile>.Update;
            var update = builder.Set(x => x.IsCreatedEventPublished, true);

            profileCollection.UpdateOne(x => x.Id == profileId, update, null);
        }
    }
}