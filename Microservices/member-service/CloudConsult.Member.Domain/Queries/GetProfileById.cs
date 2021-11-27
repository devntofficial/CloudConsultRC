using CloudConsult.Member.Domain.Responses;
using CloudConsult.Common.CQRS;

namespace CloudConsult.Member.Domain.Queries
{
    public record GetProfileById : IQuery<ProfileResponse>
    {
        public string ProfileId { get; set; }
    }
}