using CloudConsult.Common.Controllers;
using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Consultation.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
public class TimeSlotController : JsonController<TimeSlotController>
{
    [HttpPost(Routes.TimeSlot.Add)]
    public async Task<IActionResult> Add(AddTimeSlot command)
    {
        var response = await Mediator.Send(command);
        return JsonResponse(response);
    }

    [HttpGet(Routes.TimeSlot.GetByDoctorId)]
    public async Task<IActionResult> GetByDoctorId([FromRoute] GetTimeSlotsByDoctorId query)
    {
        var response = await Mediator.Send(query);
        return JsonResponse(response);
    }

    [HttpPost(Routes.TimeSlot.GetByRange)]
    public async Task<IActionResult> GetByRange([FromRoute] string ProfileId, [FromBody] GetTimeSlotsByRange query)
    {
        query.ProfileId = ProfileId;
        var response = await Mediator.Send(query);
        return JsonResponse(response);
    }
}