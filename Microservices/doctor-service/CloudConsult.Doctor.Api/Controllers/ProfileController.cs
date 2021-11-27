using CloudConsult.Common.Controllers;
using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Queries;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Doctor.Api.Controllers
{
    [ApiVersion("1.0")]
    public class ProfileController : JsonController<ProfileController>
    {
        [HttpPost(Routes.Profile.CreateProfile)]
        public async Task<IActionResult> CreateProfile(CreateProfile command)
        {
            var response = await Mediator.Send(command);
            var resourceUrl = response.IsSuccess ? $"{HttpContext.Request.GetDisplayUrl()}/{response.Payload.ProfileId}" : "";
            return response.IsSuccess ? JsonResponse(response, resourceUrl) : JsonResponse(response);
        }

        [HttpPut(Routes.Profile.UpdateProfile)]
        public async Task<IActionResult> UpdateProfile([FromRoute] string ProfileId, UpdateProfile command)
        {
            command.ProfileId = ProfileId;
            var response = await Mediator.Send(command);
            var resourceUrl = response.IsSuccess ? $"{HttpContext.Request.GetDisplayUrl()}/{response.Payload.ProfileId}" : "";
            return response.IsSuccess ? JsonResponse(response, resourceUrl) : JsonResponse(response);
        }

        [HttpGet(Routes.Profile.GetProfileById)]
        public async Task<IActionResult> GetProfileById([FromRoute] GetProfileById query)
        {
            var response = await Mediator.Send(query);
            return JsonResponse(response);
        }

        [HttpGet(Routes.Profile.GetProfileByIdentityId)]
        public async Task<IActionResult> GetProfileByIdentityId([FromRoute] GetProfileByIdentityId query)
        {
            var response = await Mediator.Send(query);
            return JsonResponse(response);
        }
    }
}