using CloudConsult.Common.Controllers;
using CloudConsult.Doctor.Domain.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Doctor.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class KycController : JsonController<KycController>
    {
        [HttpPost(Routes.Kyc.Approve)]
        [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Approve([FromRoute] string ProfileId, ApproveKyc command, CancellationToken cancellationToken = default)
        {
            command.ProfileId = ProfileId;
            var response = await Mediator.Send(command, cancellationToken);
            return JsonResponse(response);
        }

        [HttpPost(Routes.Kyc.Reject)]
        [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Reject([FromRoute] string ProfileId, RejectKyc command, CancellationToken cancellationToken = default)
        {
            command.ProfileId = ProfileId;
            var response = await Mediator.Send(command, cancellationToken);
            return JsonResponse(response);
        }
    }
}
