using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Domain.Responses;

namespace CloudConsult.Identity.Domain.Services
{
    public interface ITokenService
    {
        GetTokenResponse GenerateJwtTokenFor(User user);
        bool GenerateEmailOtpFor(User user);
        Task<bool> ValidateEmailOtp(UserOtp userOtp);
    }
}