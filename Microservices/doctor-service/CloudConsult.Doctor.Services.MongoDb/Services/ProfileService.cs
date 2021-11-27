using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Doctor.Services.MongoDb.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IMongoCollection<DoctorProfile> _profileCollection;

        public ProfileService(IMongoCollection<DoctorProfile> profileCollection)
        {
            this._profileCollection = profileCollection;
        }

        public async Task<DoctorProfile> Create(DoctorProfile profile, CancellationToken cancellationToken = default)
        {
            profile.IsActive = false;
            profile.ProfileCreatedEventPublished = false;
            profile.ProfileUpdatedEventPublished = true;
            profile.CreatedDate = DateTime.UtcNow;
            await _profileCollection.InsertOneAsync(profile, null, cancellationToken);
            return profile;
        }

        public async Task<DoctorProfile> Update(DoctorProfile profile, CancellationToken cancellationToken = default)
        {
            var filter = Builders<DoctorProfile>.Filter.Eq("Id", profile.Id);

            var builder = Builders<DoctorProfile>.Update;
            var update = builder
                .Set(x => x.ProfileUpdatedEventPublished, false)
                .Set(x => x.Address, profile.Address)
                .Set(x => x.Gender, profile.Gender)
                .Set(x => x.AadhaarNo, profile.AadhaarNo)
                .Set(x => x.EmailId, profile.EmailId)
                .Set(x => x.FullName, profile.FullName);

            var returnedDoctor = await _profileCollection
                .FindOneAndUpdateAsync<DoctorProfile>(filter, update, null, cancellationToken);

            return returnedDoctor is null ? null : profile;
        }

        public async Task<DoctorProfile> GetById(string profileId, CancellationToken cancellationToken = default)
        {
            var isValidIdFormat = ObjectId.TryParse(profileId, out var id);
            if (isValidIdFormat)
            {
                return await _profileCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
            }

            return null;
        }

        public async Task<DoctorProfile> GetByIdentityId(string identityId, CancellationToken cancellationToken = default)
        {
            return await _profileCollection.Find(x => x.IdentityId == identityId).FirstOrDefaultAsync(cancellationToken);
        }
    }
}