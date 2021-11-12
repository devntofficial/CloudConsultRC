using System.Threading.Tasks;
using CloudConsult.UI.Models;

namespace CloudConsult.UI.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticatedUserModel> Login(LoginModel userLoginData);
        Task Logout();
    }
}