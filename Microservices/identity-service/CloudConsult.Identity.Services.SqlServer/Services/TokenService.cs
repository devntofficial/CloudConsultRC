using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CloudConsult.Common.Configurations;
using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Domain.Responses;
using CloudConsult.Identity.Domain.Services;
using CloudConsult.Identity.Services.SqlServer.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CloudConsult.Identity.Services.SqlServer.Services
{
    public class TokenService : ITokenService
    {
        private readonly IdentityDbContext _db;
        private readonly JwtConfiguration _jwtConfiguration;
        
        public TokenService(IdentityDbContext db, JwtConfiguration jwtConfiguration)
        {
            _db = db;
            _jwtConfiguration = jwtConfiguration;
        }

        public bool GenerateEmailOtpFor(User user)
        {
            //generate otp and send email to user from here
            return true;
        }

        public GetTokenResponse GenerateJwtTokenFor(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(_jwtConfiguration.SecretKey);

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
                Expires = DateTime.UtcNow.AddMinutes(_jwtConfiguration.ExpiryTimeInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            return new GetTokenResponse
            {
                AccessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
                ExpiryTimestamp = tokenDescriptor.Expires.Value
            };
        }

        public async Task<bool> ValidateEmailOtp(UserOtp userOtp)
        {
            var storedOtp = await _db.UserOtps
                .FirstOrDefaultAsync(x => x.UserId == userOtp.UserId && x.IsActive && x.ExpiryTimestamp > DateTime.UtcNow);

            return storedOtp.Otp == userOtp.Otp;
        }
    }
}