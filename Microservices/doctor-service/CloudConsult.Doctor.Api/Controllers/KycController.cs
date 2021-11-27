using CloudConsult.Common.Controllers;
using CloudConsult.Doctor.Domain.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Doctor.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class KycController : JsonController<KycController>
    {
        //only admins access this route
        [HttpPost(Routes.Kyc.Approve)]
        public async Task<IActionResult> Approve([FromRoute] string ProfileId, ApproveProfile command, CancellationToken cancellationToken = default)
        {
            command.ProfileId = ProfileId;
            var response = await Mediator.Send(command, cancellationToken);
            return JsonResponse(response);
        }

        //only admins access this route
        [HttpPost(Routes.Kyc.Reject)]
        public async Task<IActionResult> Reject([FromRoute] string ProfileId, RejectProfile command, CancellationToken cancellationToken = default)
        {
            command.ProfileId = ProfileId;
            var response = await Mediator.Send(command, cancellationToken);
            return JsonResponse(response);
        }
    }
}
