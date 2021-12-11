using CloudConsult.Common.Encryption;
using CloudConsult.Identity.Domain.Commands;
using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Domain.Queries;
using CloudConsult.Identity.Domain.Services;
using CloudConsult.Identity.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CloudConsult.Identity.Services.SqlServer.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IdentityDbContext db;
        private readonly IHashingService hashingService;
        private readonly ILogger<IdentityService> logger;

        public IdentityService(IdentityDbContext db,
            IHashingService hashingService, ILogger<IdentityService> logger)
        {
            this.db = db;
            this.hashingService = hashingService;
            this.logger = logger;
        }

        public async Task<User> Authenticate(GetToken query, CancellationToken cancellationToken)
        {
            var user = await db.Users.AsNoTracking()
                .Where(x => x.EmailId == query.EmailId)
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .SingleOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                return null;
            }

            var hashedPassword = hashingService.GenerateHashWithSalt(query.Password, user.PasswordSalt);
            return hashedPassword == user.PasswordHash ? user : null;
        }

        public async Task<User> Create(CreateUser command, CancellationToken cancellationToken)
        {
            await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = new User
                {
                    FullName = command.FullName,
                    EmailId = command.EmailId,
                    Timestamp = DateTime.Now,
                    PasswordSalt = hashingService.GenerateRandomSalt()
                };
                user.PasswordHash = hashingService.GenerateHashWithSalt(command.Password, user.PasswordSalt);

                await db.Users.AddAsync(user, cancellationToken);

                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = command.RoleId,
                    Timestamp = DateTime.Now
                };

                await db.UserRoles.AddAsync(userRole, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return await db.Users
                    .Where(x => x.Id == user.Id)
                    .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                    .SingleOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                await transaction.RollbackAsync(cancellationToken);
                return null;
            }
        }
    }
}