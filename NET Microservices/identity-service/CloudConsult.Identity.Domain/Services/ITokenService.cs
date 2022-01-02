using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Domain.Responses;

namespace CloudConsult.Identity.Domain.Services
{
    public interface ITokenService
    {
        GetTokenResponse GenerateJwtTokenFor(User user);
        Task GenerateOtpFor(string userId, CancellationToken cancellationToken = default);
        Task<bool> ValidateOtp(string userId, int otp, CancellationToken cancellationToken = default);
    }
}