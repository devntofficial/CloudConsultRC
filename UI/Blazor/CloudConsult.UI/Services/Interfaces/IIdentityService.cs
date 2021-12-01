using CloudConsult.UI.Models.Identity;

namespace CloudConsult.UI.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> Login(LoginModel userLoginData);
        Task Logout();
        Task<List<UserRoleModel>> GetUserRoles();
        Task<bool> Register(UserRegistrationModel data);
    }
}