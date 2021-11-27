using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Responses;

namespace CloudConsult.Doctor.Domain.Queries
{
    public record GetProfileByIdentityId : IQuery<ProfileResponse>
    {
        public string IdentityId { get; set; }
    }
}
