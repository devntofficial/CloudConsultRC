﻿using CloudConsult.Common.Builders;
using CloudConsult.Common.Controllers;
using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Queries;
using CloudConsult.Doctor.Domain.Responses;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Doctor.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class ProfileController : JsonController<ProfileController>
    {
        [HttpPost(Routes.Profile.Create)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(IApiResponse<ProfileResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IApiResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(IApiResponse))]
        public async Task<IActionResult> Create(CreateProfile command)
        {
            var response = await Mediator.Send(command);
            var resourceUrl = response.IsSuccess ? $"{HttpContext.Request.GetDisplayUrl()}/{response.Payload.ProfileId}" : string.Empty;
            return response.IsSuccess ? JsonResponse(response, resourceUrl) : JsonResponse(response);
        }

        [HttpPut(Routes.Profile.Update)]
        public async Task<IActionResult> Update([FromRoute] string ProfileId, UpdateProfile command)
        {
            command.ProfileId = ProfileId;
            var response = await Mediator.Send(command);
            var resourceUrl = response.IsSuccess ? $"{HttpContext.Request.GetDisplayUrl()}/{response.Payload.ProfileId}" : string.Empty;
            return response.IsSuccess ? JsonResponse(response, resourceUrl) : JsonResponse(response);
        }

        [HttpGet(Routes.Profile.GetAll)]
        public async Task<IActionResult> GetAll([FromHeader] GetAllProfiles query)
        {
            var response = await Mediator.Send(query);
            return JsonResponse(response);
        }

        [HttpGet(Routes.Profile.GetById)]
        public async Task<IActionResult> GetById([FromRoute] GetProfileById query)
        {
            var response = await Mediator.Send(query);
            return JsonResponse(response);
        }

        [HttpGet(Routes.Profile.GetByIdentityId)]
        public async Task<IActionResult> GetByIdentityId([FromRoute] GetProfileByIdentityId query)
        {
            var response = await Mediator.Send(query);
            return JsonResponse(response);
        }
    }
}