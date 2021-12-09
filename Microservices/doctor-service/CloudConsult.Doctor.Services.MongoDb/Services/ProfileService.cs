using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Doctor.Services.MongoDb.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IMongoCollection<DoctorProfile> profileCollection;

        public ProfileService(IMongoCollection<DoctorProfile> profileCollection)
        {
            this.profileCollection = profileCollection;
        }

        public async Task<DoctorProfile> Create(DoctorProfile profile, CancellationToken cancellationToken = default)
        {
            profile.IsUpdatedEventPublished = true;
            await profileCollection.InsertOneAsync(profile, null, cancellationToken);
            return profile;
        }

        public async Task<DoctorProfile> Update(DoctorProfile profile, CancellationToken cancellationToken = default)
        {
            var filter = Builders<DoctorProfile>.Filter.And(
                Builders<DoctorProfile>.Filter.Eq(x => x.Id, profile.Id),
                Builders<DoctorProfile>.Filter.Eq(x => x.IdentityId, profile.IdentityId));

            var builder = Builders<DoctorProfile>.Update;
            var update = builder
                .Set(x => x.IsUpdatedEventPublished, false)
                .Set(x => x.Address, profile.Address)
                .Set(x => x.Gender, profile.Gender)
                .Set(x => x.AadhaarNo, profile.AadhaarNo)
                .Set(x => x.EmailId, profile.EmailId)
                .Set(x => x.FullName, profile.FullName)
                .Set(x => x.DateOfBirth, profile.DateOfBirth)
                .Set(x => x.MobileNo, profile.MobileNo)
                .Set(x => x.Speciality, profile.Speciality);

            var returnedDoctor = await profileCollection.FindOneAndUpdateAsync(filter, update,
                new FindOneAndUpdateOptions<DoctorProfile, DoctorProfile>
                {
                    ReturnDocument = ReturnDocument.After
                }, cancellationToken);

            return returnedDoctor;
        }

        public async Task<DoctorProfile> GetById(string profileId, CancellationToken cancellationToken = default)
        {
            var isValidIdFormat = ObjectId.TryParse(profileId, out var id);
            if (isValidIdFormat)
            {
                return await profileCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
            }

            return null;
        }

        public async Task<DoctorProfile> GetByIdentityId(string identityId, CancellationToken cancellationToken = default)
        {
            return await profileCollection.Find(x => x.IdentityId == identityId).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<DoctorProfile>> GetAllPaginated(int pageNo, int pageSize, CancellationToken cancellationToken = default)
        {
            var filter = Builders<DoctorProfile>.Filter.Eq(x => x.IsActive, true);
            return await profileCollection.Find(filter).SortByDescending(x => x.Timestamp)
                .Skip((pageNo - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();
        }
    }
}