using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Domain.Services;
using CloudConsult.Identity.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudConsult.Identity.Services.SqlServer.Services
{
    public class RoleService : IRoleService
    {
        private readonly IdentityDbContext db;
        private readonly ILogger<RoleService> logger;

        public RoleService(IdentityDbContext db, ILogger<RoleService> logger)
        {
            this.db = db;
            this.logger = logger;
        }

        public async Task<List<Role>> GetUserRoles(CancellationToken cancellationToken = default)
        {
            return await db.Roles.ToListAsync(cancellationToken);
        }
    }
}
