using System.Threading;
using System.Threading.Tasks;
using CloudConsult.Identity.Domain.Commands;
using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Domain.Queries;

namespace CloudConsult.Identity.Domain.Services
{
    public interface IIdentityService
    {
        Task<UserEntity> AuthenticateUser(GetTokenQuery query, CancellationToken cancellationToken);
        Task<UserEntity> CreateUser(CreateUserCommand command, CancellationToken cancellationToken);
    }
}