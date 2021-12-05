using CloudConsult.Doctor.Domain.Commands;

namespace CloudConsult.Doctor.Domain.Services
{
    public interface IKycService
    {
        Task<bool> Approve(ApproveKyc command, CancellationToken cancellationToken = default);
        Task<bool> Reject(RejectKyc command, CancellationToken cancellationToken = default);
    }
}
