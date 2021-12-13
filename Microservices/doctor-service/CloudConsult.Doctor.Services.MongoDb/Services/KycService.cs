using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Doctor.Services.MongoDb.Services
{
    public class KycService : IKycService
    {
        private readonly IMongoCollection<DoctorProfile> profileCollection;
        private readonly IMongoCollection<DoctorKyc> kycCollection;

        public KycService(IMongoCollection<DoctorProfile> profileCollection, IMongoCollection<DoctorKyc> kycCollection)
        {
            this.profileCollection = profileCollection;
            this.kycCollection = kycCollection;
        }

        public async Task<bool> Approve(ApproveKyc command, CancellationToken cancellationToken = default)
        {
            var profile = await profileCollection.Find(x => x.Id == command.ProfileId).FirstAsync(cancellationToken);

            if (profile is null) return false;

            await kycCollection.InsertOneAsync(new DoctorKyc
            {
                ProfileId = command.ProfileId,
                EmailId = profile.EmailId,
                FullName = profile.FullName,
                IsApproved = true,
                AdministratorId = command.ApprovalIdentityId,
                Comments = command.ApprovalComments
            }, null, cancellationToken);

            var builder = Builders<DoctorProfile>.Update;
            var update = builder.Set(x => x.IsActive, true);

            var result = await profileCollection.UpdateOneAsync(x => x.Id == command.ProfileId, update, null, cancellationToken);
            if(result.IsModifiedCountAvailable is false)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Reject(RejectKyc command, CancellationToken cancellationToken = default)
        {
            var profile = await profileCollection.Find(x => x.Id == command.ProfileId).FirstAsync(cancellationToken);

            if (profile is null) return false;

            await kycCollection.InsertOneAsync(new DoctorKyc
            {
                ProfileId = command.ProfileId,
                EmailId = profile.EmailId,
                FullName = profile.FullName,
                IsApproved = false,
                AdministratorId = command.RejectionIdentityId,
                Comments = command.RejectionComments
            }, null, cancellationToken);

            var builder = Builders<DoctorProfile>.Update;
            var update = builder.Set(x => x.IsActive, false);

            var result = await profileCollection.UpdateOneAsync(x => x.Id == command.ProfileId, update, null, cancellationToken);
            if (result.IsModifiedCountAvailable is false)
            {
                return false;
            }

            return true;
        }
    }
}
