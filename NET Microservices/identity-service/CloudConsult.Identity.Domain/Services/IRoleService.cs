using CloudConsult.Identity.Domain.Entities;

namespace CloudConsult.Identity.Domain.Services
{
    public interface IRoleService
    {
        Task<List<Role>> GetUserRoles(CancellationToken cancellationToken = default);
    }
}
