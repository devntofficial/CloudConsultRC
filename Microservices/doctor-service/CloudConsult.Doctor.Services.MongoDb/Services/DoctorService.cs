using System;
using System.Threading;
using System.Threading.Tasks;
using CloudConsult.Doctor.Domain.Entities;
using CloudConsult.Doctor.Domain.Services;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CloudConsult.Doctor.Services.MongoDb.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IMongoCollection<DoctorEntity> _doctorCollection;

        public DoctorService(IMongoCollection<DoctorEntity> doctorCollection)
        {
            this._doctorCollection = doctorCollection;
        }
        
        public async Task<DoctorEntity> Create(DoctorEntity doctor, CancellationToken cancellationToken = default)
        {
            doctor.IsActive = false;
            doctor.IsRegistrationEventPublished = false;
            doctor.IsUpdationEventPublished = true;
            doctor.CreatedDate = DateTime.UtcNow;
            await _doctorCollection.InsertOneAsync(doctor, null, cancellationToken).ConfigureAwait(false);
            return doctor;
        }

        public async Task<DoctorEntity> GetById(string doctorId, CancellationToken cancellationToken = default)
        {
            var isValidIdFormat = ObjectId.TryParse(doctorId, out var id);
            if (isValidIdFormat)
            {
                return await _doctorCollection.Find(x => x.Id == id)
                    .FirstOrDefaultAsync(cancellationToken)
                    .ConfigureAwait(false);
            }

            return null;
        }

        public async Task<DoctorEntity> UpdateDoctor(DoctorEntity doctor, CancellationToken cancellationToken = default)
        {
            var filter = Builders<DoctorEntity>.Filter.Eq("Id", doctor.Id);
            
            var builder = Builders<DoctorEntity>.Update;
            var update = builder
                .Set(x => x.IsUpdationEventPublished, false)
                .Set(x => x.Address, doctor.Address)
                .Set(x => x.Gender, doctor.Gender)
                .Set(x => x.AadhaarNo, doctor.AadhaarNo)
                .Set(x => x.EmailId, doctor.EmailId)
                .Set(x => x.FirstName, doctor.FirstName)
                .Set(x => x.LastName, doctor.LastName);
            
            var returnedDoctor = await _doctorCollection
                .FindOneAndUpdateAsync<DoctorEntity>(filter, update, null, cancellationToken)
                .ConfigureAwait(false);
            
            return returnedDoctor is null ? null : doctor;
        }
    }
}