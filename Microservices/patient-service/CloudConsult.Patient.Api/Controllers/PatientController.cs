using CloudConsult.Patient.Domain.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using CloudConsult.Common.Controllers;

namespace CloudConsult.Patient.Api.Controllers
{
    [ApiVersion("1.0")]
    public class PatientController : JsonController<PatientController>
    {
        [HttpPost(Routes.Patient.CreatePatient)]
        public async Task<IActionResult> CreatePatient(CreatePatientCommand command, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(command, cancellationToken);
            return JsonResponse(response);
        }
    }
}
