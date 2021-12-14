using CloudConsult.Common.Controllers;
using CloudConsult.Payment.Domain.Commands;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Payment.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class PaymentController : JsonController<PaymentController>
    {
        [HttpPost(Routes.Payment.Accept)]
        public async Task<IActionResult> Accept([FromRoute] string ConsultationId, CancellationToken cancellationToken = default)
        {
            var command = new AcceptPayment { ConsultationId = ConsultationId };
            return JsonResponse(await Mediator.Send(command, cancellationToken));
        }

        [HttpPost(Routes.Payment.Reject)]
        public async Task<IActionResult> Reject([FromRoute] string ConsultationId, CancellationToken cancellationToken = default)
        {
            var command = new RejectPayment { ConsultationId = ConsultationId };
            return JsonResponse(await Mediator.Send(command, cancellationToken));
        }
    }
}
