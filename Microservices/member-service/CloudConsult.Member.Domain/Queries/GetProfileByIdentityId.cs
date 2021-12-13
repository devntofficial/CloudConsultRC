using CloudConsult.Common.CQRS;
using CloudConsult.Member.Domain.Responses;

namespace CloudConsult.Member.Domain.Queries
{
    public class GetProfileByIdentityId : IQuery<ProfileResponse>
    {
        public string IdentityId { get; set; }
    }
}
