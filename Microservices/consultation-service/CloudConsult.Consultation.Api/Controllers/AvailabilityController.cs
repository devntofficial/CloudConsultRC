using System.Threading.Tasks;
using CloudConsult.Common.Controllers;
using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Consultation.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class AvailabilityController : JsonController<AvailabilityController>
    {
        [HttpPost(Routes.Availability.Add)]
        public async Task<IActionResult> Add(AddAvailability command)
        {
            var response = await Mediator.Send(command);
            return JsonResponse(response);
        }
        
        [HttpGet(Routes.Availability.GetByDoctorId)]
        public async Task<IActionResult> GetByDoctorId([FromRoute]GetAvailabilityByDoctorId query)
        {
            var response = await Mediator.Send(query);
            return JsonResponse(response);
        }
    }
}