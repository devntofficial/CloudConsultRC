using CloudConsult.Common.CQRS;
using CloudConsult.Identity.Domain.Entities;

namespace CloudConsult.Identity.Domain.Queries
{
    public class GetUserRoles : IQuery<List<Role>>
    {
    }
}
