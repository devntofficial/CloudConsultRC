using CloudConsult.Patient.Domain.Entities;
using CloudConsult.Patient.Domain.Services;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CloudConsult.Patient.Services.MongoDb.Services
{
    public class PatientService : IPatientService
    {
        private readonly IMongoCollection<PatientEntity> repo;

        public PatientService(IMongoCollection<PatientEntity> repo)
        {
            this.repo = repo;
        }

        public async Task<PatientEntity> CreatePatient(PatientEntity patient, CancellationToken cancellationToken)
        {
            patient.IsRegistrationEventPublished = false;
            patient.IsUpdationEventPublished = true;
            patient.CreatedDate = DateTime.UtcNow;
            await repo.InsertOneAsync(patient, null, cancellationToken);
            return patient;
        }
    }
}
