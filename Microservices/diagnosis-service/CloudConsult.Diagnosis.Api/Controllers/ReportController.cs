using CloudConsult.Common.Controllers;
using CloudConsult.Diagnosis.Domain.Commands;
using CloudConsult.Diagnosis.Domain.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Diagnosis.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class ReportController : JsonController<ReportController>
    {
        [HttpPost(Routes.Report.Upload)]
        public async Task<IActionResult> Upload([FromRoute] string ConsultationId, UploadReport command,
            CancellationToken cancellationToken = default)
        {
            command.ConsultationId = ConsultationId;
            var response = await Mediator.Send(command, cancellationToken);

            if (response.IsSuccess)
            {
                var resourceUrl = $"{Request.Scheme}://{Request.Host.Value}/{Routes.Report.GetById}"
                    .Replace("{ReportId}", response.Payload?.ReportId);

                return JsonResponse(response, resourceUrl);
            }

            return JsonResponse(response);
        }

        [HttpGet(Routes.Report.GetById)]
        public async Task<IActionResult> GetById([FromRoute] string ReportId, CancellationToken cancellationToken = default)
        {
            var query = new GetReportById(ReportId);
            var response = await Mediator.Send(query, cancellationToken);
            return JsonResponse(response);
        }

        [HttpGet(Routes.Report.GetByConsultationId)]
        public async Task<IActionResult> GetByConsultationId([FromRoute] string ConsultationId, CancellationToken cancellationToken = default)
        {
            var query = new GetReportByConsultationId(ConsultationId);
            var response = await Mediator.Send(query, cancellationToken);
            return JsonResponse(response);
        }
    }
}