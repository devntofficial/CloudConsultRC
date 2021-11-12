using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CloudConsult.Doctor.Domain.Entities;
using MongoDB.Bson;

namespace CloudConsult.Doctor.Domain.Services
{
    public interface IDoctorService
    {
        Task<DoctorEntity> Create(DoctorEntity doctor, CancellationToken cancellationToken = default);
        Task<DoctorEntity> GetById(string doctorId, CancellationToken cancellationToken = default);
        Task<DoctorEntity> UpdateDoctor(DoctorEntity doctor, CancellationToken cancellationToken = default);
    }
}