using CloudConsult.Doctor.Domain.Responses;
using CloudConsult.Common.CQRS;

namespace CloudConsult.Doctor.Domain.Queries
{
    public class GetProfileById : IQuery<ProfileResponse>
    {
        public string ProfileId { get; set; }
    }
}