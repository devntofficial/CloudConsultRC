using CloudConsult.Common.Controllers;
using CloudConsult.Member.Domain.Commands;
using CloudConsult.Member.Domain.Queries;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Member.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class ProfileController : JsonController<ProfileController>
    {
        [HttpPost(Routes.Profile.Create)]
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