using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Queries;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CloudConsult.Common.Controllers;

namespace CloudConsult.Doctor.Api.Controllers
{
    [ApiVersion("1.0")]
    public class DoctorController : JsonController<DoctorController>
    {
        [HttpPost(Routes.Doctor.CreateDoctor)]
        public async Task<IActionResult> CreateDoctor(CreateDoctorCommand command)
        {
            var response = await Mediator.Send(command);
            var resourceUrl = response.IsSuccess ? $"{HttpContext.Request.GetDisplayUrl()}/{response.Payload.DoctorId}" : "";
            return response.IsSuccess ? JsonResponse(response, resourceUrl) : JsonResponse(response);
        }

        [HttpGet(Routes.Doctor.GetDoctorById)]
        public async Task<IActionResult> GetDoctorById([FromRoute] GetDoctorByIdQuery query)
        {
            var response = await Mediator.Send(query);
            return JsonResponse(response);
        }

        [HttpPut(Routes.Doctor.UpdateDoctor)]
        public async Task<IActionResult> UpdateDoctor([FromRoute] string DoctorId, UpdateDoctorCommand command)
        {
            command.DoctorId = DoctorId;
            var response = await Mediator.Send(command);
            var resourceUrl = response.IsSuccess ? $"{HttpContext.Request.GetDisplayUrl()}/{response.Payload.DoctorId}" : "";
            return response.IsSuccess ? JsonResponse(response, resourceUrl) : JsonResponse(response);
        }
    }
}