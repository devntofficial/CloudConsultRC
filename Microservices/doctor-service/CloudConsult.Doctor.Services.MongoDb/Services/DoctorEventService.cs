using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Doctor.Services.MongoDb.Services
{
    public class DoctorEventService : IDoctorEventService
    {
        private readonly IMongoCollection<DoctorEntity> _doctorCollection;

        public DoctorEventService(IMongoCollection<DoctorEntity> doctorCollection)
        {
            this._doctorCollection = doctorCollection;
        }
        
        public async Task<IEnumerable<DoctorEntity>> GetUnpublishedCreatedDoctors(CancellationToken cancellationToken = default)
        {
            return await _doctorCollection.Find(x => x.IsRegistrationEventPublished == false)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<DoctorEntity>> GetUnpublishedUpdatedDoctors(CancellationToken cancellationToken = default)
        {
            return await _doctorCollection.Find(x => x.IsUpdationEventPublished == false)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task UpdateRegistrationEventPublished(ObjectId doctorId, CancellationToken cancellationToken = default)
        {
            var builder = Builders<DoctorEntity>.Update;
            var update = builder.Set(x => x.IsRegistrationEventPublished, true);

            await _doctorCollection.UpdateOneAsync(x => x.Id == doctorId, update, null, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task UpdateUpdationEventPublished(ObjectId doctorId, CancellationToken cancellationToken = default)
        {
            var builder = Builders<DoctorEntity>.Update;
            var update = builder.Set(x => x.IsUpdationEventPublished, true);

            await _doctorCollection.UpdateOneAsync(x => x.Id == doctorId, update, null, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}