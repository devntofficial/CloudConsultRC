using AutoMapper;
using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Domain.Events;
using CloudConsult.Identity.Domain.Responses;

namespace CloudConsult.Identity.Infrastructure.Mappers
{
    public class IdentityMapper : Profile
    {
        public IdentityMapper()
        {
            CreateMap<User, CreateUserResponse>()
                .ForMember(x => x.Roles, y => y.MapFrom(z => string.Join(',', z.UserRoles.Select(r => r.Role.RoleName))));

            CreateMap<UserOtp, OtpGenerated>().ConstructUsing(x => OtpGeneratedMapper(x));
        }

        private static OtpGenerated OtpGeneratedMapper(UserOtp userOtp)
        {
            return new OtpGenerated
            {
                EventId = userOtp.Id,
                EmailId = userOtp.User.EmailId,
                FullName = userOtp.User.FullName,
                IdentityId = userOtp.UserId,
                Otp = userOtp.Otp
            };
        }
    }
}