using AutoMapper;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Events;
using CloudConsult.Doctor.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Doctor.Services.MongoDb.Services
{
    public class EventService : IEventService
    {
        private readonly IMongoCollection<DoctorProfile> profileCollection;
        private readonly IMongoCollection<DoctorKyc> kycCollection;
        private readonly IMapper mapper;

        public EventService(IMongoCollection<DoctorProfile> profileCollection, IMongoCollection<DoctorKyc> kycCollection, IMapper mapper)
        {
            this.profileCollection = profileCollection;
            this.kycCollection = kycCollection;
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
            var builder = Builders<DoctorProfile>.Update;
            var update = builder.Set(x => x.IsUpdatedEventPublished, true);

            profileCollection.UpdateOne(x => x.Id == profileId, update, null);
        }

        public void SetProfileCreatedEventPublished(string profileId)
        {
            var builder = Builders<DoctorProfile>.Update;
            var update = builder.Set(x => x.IsCreatedEventPublished, true);

            profileCollection.UpdateOne(x => x.Id == profileId, update, null);
        }

        public async Task<IEnumerable<KycApproved>> GetPendingKycApprovedEvents(CancellationToken cancellationToken = default)
        {
            var events = await kycCollection.Find(x => !x.IsEventPublished && x.IsApproved).ToListAsync(cancellationToken);
            return events.Count > 0 ? mapper.Map<List<KycApproved>>(events) : Enumerable.Empty<KycApproved>();
        }

        public async Task<IEnumerable<KycRejected>> GetPendingKycRejectedEvents(CancellationToken cancellationToken = default)
        {
            var events = await kycCollection.Find(x => !x.IsEventPublished && !x.IsApproved).ToListAsync(cancellationToken);
            return events.Count > 0 ? mapper.Map<List<KycRejected>>(events) : Enumerable.Empty<KycRejected>();
        }

        public void SetKycEventPublished(string eventId)
        {
            var builder = Builders<DoctorKyc>.Update;
            var update = builder.Set(x => x.IsEventPublished, true);

            kycCollection.UpdateOne(x => x.Id == eventId, update, null);
        }
    }
}