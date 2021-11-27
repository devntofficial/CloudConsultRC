using AutoMapper;
using CloudConsult.Member.Domain.Commands;
using CloudConsult.Member.Domain.Entities;
using CloudConsult.Member.Domain.Events;
using CloudConsult.Member.Domain.Responses;

namespace CloudConsult.Member.Infrastructure.Mappers
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<CreateProfile, MemberProfile>();
            CreateMap<MemberProfile, ProfileResponse>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id.ToString()));

            CreateMap<UpdateProfile, MemberProfile>();
            CreateMap<MemberProfile, ProfileResponse>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id.ToString()));

            CreateMap<MemberProfile, ProfileCreated>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id.ToString()));
            CreateMap<MemberProfile, ProfileUpdated>()
                .ForMember(x => x.ProfileId, y => y.MapFrom(z => z.Id.ToString()));
        }
    }
}