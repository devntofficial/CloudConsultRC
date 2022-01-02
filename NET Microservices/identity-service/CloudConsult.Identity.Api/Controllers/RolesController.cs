using CloudConsult.Common.Controllers;
using CloudConsult.Identity.Domain.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Identity.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class RolesController : JsonController<RolesController>
    {
        [HttpGet(Routes.Roles.GetUserRoles)]
        public async Task<IActionResult> GetUserRoles(CancellationToken cancellationToken)
        {
            var query = new GetUserRoles();
            var response = await Mediator.Send(query, cancellationToken);
            return JsonResponse(response);
        }
    }
}
