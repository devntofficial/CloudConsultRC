using CloudConsult.Doctor.Domain.Commands;

namespace CloudConsult.Doctor.Domain.Services
{
    public interface IKycService
    {
        Task<bool> Approve(ApproveProfile command, CancellationToken cancellationToken = default);
        Task<bool> Reject(RejectProfile command, CancellationToken cancellationToken = default);
    }
}
