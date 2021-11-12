using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Domain.Responses;

namespace CloudConsult.Identity.Domain.Services
{
    public interface ITokenService
    {
        GetTokenResponse GenerateJwtTokenFor(UserEntity user);
    }
}