using CloudConsult.Common.CQRS;
using CloudConsult.Doctor.Domain.Responses;

namespace CloudConsult.Doctor.Domain.Queries
{
    public class GetAllProfiles : IPaginatedQuery<List<ProfileResponse>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }
    }
}
