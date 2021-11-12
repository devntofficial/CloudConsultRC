using CloudConsult.Patient.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace CloudConsult.Patient.Domain.Services
{
    public interface IPatientService
    {
        Task<PatientEntity> CreatePatient(PatientEntity patient, CancellationToken cancellationToken);
    }
}
