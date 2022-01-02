using CloudConsult.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloudConsult.Identity.Services.SqlServer.Contexts
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserOtp> UserOtps { get; set; }
    }
}