using System.Threading;
using System.Threading.Tasks;
using CloudConsult.Common.Controllers;
using CloudConsult.Identity.Domain.Commands;
using CloudConsult.Identity.Domain.Queries;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Identity.Api.Controllers
{
    [ApiVersion("1.0")]
    public class IdentityController : JsonController<IdentityController>
    {
        [HttpGet(Routes.Identity.GetToken)]
        public async Task<IActionResult> GetToken([FromHeader] string EmailId, [FromHeader] string Password, CancellationToken cancellationToken)
        {
            var query = new GetToken(EmailId, Password);
            var response = await Mediator.Send(query, cancellationToken);
            return JsonResponse(response);
        }

        [HttpPost(Routes.Identity.CreateUser)]
        public async Task<IActionResult> CreateUser(CreateUser command, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(command, cancellationToken);
            var resourceUrl = response.IsSuccess ? $"{HttpContext.Request.GetDisplayUrl()}/{response.Payload.Id}" : "";
            return response.IsSuccess ? JsonResponse(response, resourceUrl) : JsonResponse(response);
        }
    }
}