using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Doctor.Services.MongoDb.Services
{
    public class KycService : IKycService
    {
        private readonly IMongoCollection<DoctorProfile> _profileCollection;

        public KycService(IMongoCollection<DoctorProfile> profileCollection)
        {
            this._profileCollection = profileCollection;
        }

        public async Task<bool> Approve(ApproveProfile command, CancellationToken cancellationToken = default)
        {
            var isValidId = ObjectId.TryParse(command.ProfileId, out var profileId);
            if (isValidId is false)
            {
                return false;
            }

            var builder = Builders<DoctorProfile>.Update;
            var update = builder
                .Set(x => x.IsActive, true)
                .Set(x => x.ApprovalIdentityId, command.ApprovalIdentityId)
                .Set(x => x.ApprovalComments, command.ApprovalComments);

            var result = await _profileCollection.UpdateOneAsync(x => x.Id == profileId, update, null, cancellationToken);
            if(result.IsModifiedCountAvailable is false)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Reject(RejectProfile command, CancellationToken cancellationToken = default)
        {
            var isValidId = ObjectId.TryParse(command.ProfileId, out var profileId);
            if (isValidId is false)
            {
                return false;
            }

            var builder = Builders<DoctorProfile>.Update;
            var update = builder
                .Set(x => x.IsActive, false)
                .Set(x => x.RejectionIdentityId, command.RejectionIdentityId)
                .Set(x => x.RejectionComments, command.RejectionComments);

            var result = await _profileCollection.UpdateOneAsync(x => x.Id == profileId, update, null, cancellationToken);
            if (result.IsModifiedCountAvailable is false)
            {
                return false;
            }

            return true;
        }
    }
}
