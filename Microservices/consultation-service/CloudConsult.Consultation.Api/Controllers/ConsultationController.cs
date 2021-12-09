﻿using CloudConsult.Common.Controllers;
using CloudConsult.Consultation.Domain.Commands;
using CloudConsult.Consultation.Domain.Queries;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Consultation.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
public class ConsultationController : JsonController<ConsultationController>
{
    [HttpPost(Routes.Consultation.Book)]
    public async Task<IActionResult> Book(BookConsultation command, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(command, cancellationToken);
        return response.IsSuccess ?
            JsonResponse(response, $"{HttpContext.Request.GetDisplayUrl()}/{response.Payload}") :
            JsonResponse(response);
    }

    [HttpGet(Routes.Consultation.GetById)]
    public async Task<IActionResult> GetById([FromRoute] GetConsultationById query, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(query, cancellationToken);
        return JsonResponse(response);
    }

    [HttpGet(Routes.Consultation.GetByDoctorId)]
    public async Task<IActionResult> GetByDoctorId([FromRoute] GetConsultationsByDoctorId query, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(query, cancellationToken);
        return JsonResponse(response);
    }
}