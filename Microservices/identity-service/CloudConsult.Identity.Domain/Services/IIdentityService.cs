using CloudConsult.Identity.Domain.Commands;
using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Domain.Queries;

namespace CloudConsult.Identity.Domain.Services
{
    public interface IIdentityService
    {
        Task<User> Authenticate(GetToken query, CancellationToken cancellationToken);
        Task<User> Create(CreateUser command, CancellationToken cancellationToken);
    }
}