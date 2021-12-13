using CloudConsult.Member.Domain.Entities;
using CloudConsult.Member.Domain.Services;
using MongoDB.Driver;

namespace CloudConsult.Member.Services.MongoDb.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IMongoCollection<MemberProfile> profileCollection;

        public ProfileService(IMongoCollection<MemberProfile> profileCollection)
        {
            this.profileCollection = profileCollection;
        }

        public async Task<MemberProfile> Create(MemberProfile profile, CancellationToken cancellationToken = default)
        {
            profile.IsActive = true;
            profile.IsUpdatedEventPublished = true;
            await profileCollection.InsertOneAsync(profile, null, cancellationToken);
            return profile;
        }

        public async Task<MemberProfile> Update(MemberProfile profile, CancellationToken cancellationToken = default)
        {
            var filter = Builders<MemberProfile>.Filter.And(
                Builders<MemberProfile>.Filter.Eq(x => x.Id, profile.Id),
                Builders<MemberProfile>.Filter.Eq(x => x.IdentityId, profile.IdentityId));

            var builder = Builders<MemberProfile>.Update;
            var update = builder
                .Set(x => x.IsUpdatedEventPublished, false)
                .Set(x => x.Address, profile.Address)
                .Set(x => x.Gender, profile.Gender)
                .Set(x => x.EmailId, profile.EmailId)
                .Set(x => x.FullName, profile.FullName)
                .Set(x => x.DateOfBirth, profile.DateOfBirth)
                .Set(x => x.MobileNo, profile.MobileNo);

            var returnedDoctor = await profileCollection.FindOneAndUpdateAsync(filter, update,
                new FindOneAndUpdateOptions<MemberProfile, MemberProfile>
                {
                    ReturnDocument = ReturnDocument.After
                }, cancellationToken);

            return returnedDoctor;
        }

        public async Task<MemberProfile> GetById(string profileId, CancellationToken cancellationToken = default)
        {
            return await profileCollection.Find(x => x.Id == profileId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<MemberProfile> GetByIdentityId(string identityId, CancellationToken cancellationToken = default)
        {
            return await profileCollection.Find(x => x.IdentityId == identityId).FirstOrDefaultAsync(cancellationToken);
        }
    }
}