using CloudConsult.Member.Domain.Responses;
using CloudConsult.Common.CQRS;

namespace CloudConsult.Member.Domain.Queries
{
    public class GetProfileById : IQuery<ProfileResponse>
    {
        public string ProfileId { get; set; }
    }
}