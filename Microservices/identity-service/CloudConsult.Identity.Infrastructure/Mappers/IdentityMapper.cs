using System.Linq;
using AutoMapper;
using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Domain.Responses;

namespace CloudConsult.Identity.Infrastructure.Mappers
{
    public class IdentityMapper : Profile
    {
        public IdentityMapper()
        {
            CreateMap<User, CreateUserResponse>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id.ToString()))
                .ForMember(x => x.Roles,
                    y => y.MapFrom(z => string.Join(',', z.UserRoles.Select(r => r.Role.RoleName))));
        }
        
    }
}