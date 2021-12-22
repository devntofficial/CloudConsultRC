using CloudConsult.UI.Data.Authentication;
using CloudConsult.UI.Data.Common;

namespace CloudConsult.UI.Interfaces.Identity
{
    public interface IIdentityService
    {
        Task<ApiResponse<LoginResponseData>> GetToken(LoginData loginData);
        Task<ApiResponse<List<RoleData>>> GetUserRoles();
        Task<ApiResponse<RegisterResponseData>> Register(RegisterData registerData);
        Task<ApiResponse> GenerateOtp(string identityId);
        Task<ApiResponse> ValidateOtp(string identityId, int otp);
    }
}