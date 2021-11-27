using CloudConsult.Member.Domain.Entities;
using CloudConsult.Member.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Member.Services.MongoDb.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IMongoCollection<MemberProfile> _profileCollection;

        public ProfileService(IMongoCollection<MemberProfile> profileCollection)
        {
            this._profileCollection = profileCollection;
        }

        public async Task<MemberProfile> Create(MemberProfile profile, CancellationToken cancellationToken = default)
        {
            profile.IsActive = false;
            profile.ProfileCreatedEventPublished = false;
            profile.ProfileUpdatedEventPublished = true;
            profile.CreatedDate = DateTime.UtcNow;
            await _profileCollection.InsertOneAsync(profile, null, cancellationToken);
            return profile;
        }

        public async Task<MemberProfile> Update(MemberProfile profile, CancellationToken cancellationToken = default)
        {
            var filter = Builders<MemberProfile>.Filter.Eq("Id", profile.Id);

            var builder = Builders<MemberProfile>.Update;
            var update = builder
                .Set(x => x.ProfileUpdatedEventPublished, false)
                .Set(x => x.Address, profile.Address)
                .Set(x => x.Gender, profile.Gender)
                .Set(x => x.AadhaarNo, profile.AadhaarNo)
                .Set(x => x.EmailId, profile.EmailId)
                .Set(x => x.FullName, profile.FullName);

            var returnedDoctor = await _profileCollection
                .FindOneAndUpdateAsync<MemberProfile>(filter, update, null, cancellationToken);

            return returnedDoctor is null ? null : profile;
        }

        public async Task<MemberProfile> GetById(string profileId, CancellationToken cancellationToken = default)
        {
            var isValidIdFormat = ObjectId.TryParse(profileId, out var id);
            if (isValidIdFormat)
            {
                return await _profileCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
            }

            return null;
        }

        public async Task<MemberProfile> GetByIdentityId(string identityId, CancellationToken cancellationToken = default)
        {
            return await _profileCollection.Find(x => x.IdentityId == identityId).FirstOrDefaultAsync(cancellationToken);
        }
    }
}