using CloudConsult.Common.Builders;
using CloudConsult.Common.CQRS;
using CloudConsult.Identity.Domain.Entities;
using CloudConsult.Identity.Domain.Queries;
using CloudConsult.Identity.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CloudConsult.Identity.Infrastructure.Processors
{
    public class GetUserRolesProcessor : IQueryProcessor<GetUserRoles, List<Role>>
    {
        private readonly IApiResponseBuilder<List<Role>> builder;
        private readonly IRoleService roleService;

        public GetUserRolesProcessor(IApiResponseBuilder<List<Role>> builder, IRoleService roleService)
        {
            this.builder = builder;
            this.roleService = roleService;
        }

        public async Task<IApiResponse<List<Role>>> Handle(GetUserRoles request, CancellationToken cancellationToken)
        {
            var roles = await roleService.GetUserRoles(cancellationToken);

            if (roles is null || roles.Count() == 0)
            {
                return builder.CreateErrorResponse(null, x =>
                {
                    x.WithErrors("No user roles found on the server");
                    x.WithErrorCode(StatusCodes.Status404NotFound);
                });
            }

            return builder.CreateSuccessResponse(roles, x =>
            {
                x.WithMessages("User roles fetched from server successfully");
                x.WithSuccessCode(StatusCodes.Status200OK);
            });
        }
    }
}
