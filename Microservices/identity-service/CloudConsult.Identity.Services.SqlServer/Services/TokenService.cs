using CloudConsult.Common.Configurations;
using CloudConsult.Common.Encryption;
using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Domain.Responses;
using CloudConsult.Identity.Domain.Services;
using CloudConsult.Identity.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CloudConsult.Identity.Services.SqlServer.Services
{
    public class TokenService : ITokenService
    {
        private readonly IdentityDbContext db;
        private readonly JwtConfiguration jwtConfiguration;
        private readonly IHashingService hashingService;

        public TokenService(IdentityDbContext db, JwtConfiguration jwtConfiguration, IHashingService hashingService)
        {
            this.db = db;
            this.jwtConfiguration = jwtConfiguration;
            this.hashingService = hashingService;
        }

        public GetTokenResponse GenerateJwtTokenFor(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(jwtConfiguration.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.FullName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.EmailId),
                    new Claim("Roles", string.Join(',', user.UserRoles.Select(x => x.Role.RoleName)))
                }),
                Expires = DateTime.UtcNow.AddMinutes(jwtConfiguration.ExpiryTimeInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            return new GetTokenResponse
            {
                AccessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
                ExpiryTimestamp = tokenDescriptor.Expires.Value
            };
        }

        public async Task GenerateOtpFor(Guid userId, CancellationToken cancellationToken = default)
        {
            var userOtp = await db.UserOtps.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
            var otp = hashingService.GenerateRandomOtp(6);

            if (userOtp is null)
            {
                await db.UserOtps.AddAsync(new UserOtp
                {
                    UserId = userId,
                    Otp = otp
                }, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);
                return;
            }

            userOtp.Otp = otp;
            userOtp.ExpiryTimestamp = DateTime.UtcNow.AddMinutes(5);
            userOtp.IsEventPublished = false;
            await db.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> ValidateOtp(Guid userId, int otp, CancellationToken cancellationToken = default)
        {
            var userOtp = await db.UserOtps
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.Otp == otp, cancellationToken);
            
            if (userOtp is null) return false;

            db.UserOtps.Remove(userOtp);

            if(userOtp.ExpiryTimestamp > DateTime.UtcNow)
            {
                userOtp.User.IsVerified = true;
            }

            await db.SaveChangesAsync(cancellationToken);

            return userOtp.ExpiryTimestamp > DateTime.UtcNow;
        }
    }
}