using CloudConsult.UI.Helpers;
using CloudConsult.UI.Models.Identity;

namespace CloudConsult.UI.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<ApiResponse<TokenModel>> GetToken(LoginModel userLoginData);
        Task<ApiResponse<List<RoleModel>>> GetUserRoles();
        Task<ApiResponse> Register(RegistrationModel data);
        Task<ApiResponse> GenerateOtp(string identityId);
        Task<ApiResponse> ValidateOtp(string identityId, int otp);
    }
}