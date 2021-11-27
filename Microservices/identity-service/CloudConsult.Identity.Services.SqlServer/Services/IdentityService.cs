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
        private readonly IdentityDbContext _db;
        private readonly IHashingService _hashingService;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(IdentityDbContext db,
            IHashingService hashingService, ILogger<IdentityService> logger)
        {
            _db = db;
            _hashingService = hashingService;
            _logger = logger;
        }

        public async Task<User> Authenticate(GetToken query, CancellationToken cancellationToken)
        {
            var user = await _db.Users.AsNoTracking()
                .Where(x => x.EmailId == query.EmailId)
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .SingleOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                return null;
            }

            var hashedPassword = _hashingService.GenerateHashWithSalt(query.Password, user.PasswordSalt);
            return hashedPassword == user.PasswordHash ? user : null;
        }

        public async Task<User> Create(CreateUser command, CancellationToken cancellationToken)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var user = new User
                {
                    FullName = command.FullName,
                    EmailId = command.EmailId,
                    Timestamp = DateTime.UtcNow,
                    PasswordSalt = _hashingService.GenerateRandomSalt()
                };
                user.PasswordHash = _hashingService.GenerateHashWithSalt(command.Password, user.PasswordSalt);

                await _db.Users.AddAsync(user, cancellationToken);

                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = command.RoleId,
                    Timestamp = DateTime.UtcNow
                };

                await _db.UserRoles.AddAsync(userRole, cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return await _db.Users
                    .Where(x => x.Id == user.Id)
                    .Include(x => x.UserRoles)
                    .ThenInclude(x => x.Role)
                    .SingleOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                await transaction.RollbackAsync(cancellationToken);
                return null;
            }
        }
    }
}