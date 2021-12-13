using CloudConsult.Common.Controllers;
using CloudConsult.Member.Domain.Commands;
using CloudConsult.Member.Domain.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Member.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    public class ProfileController : JsonController<ProfileController>
    {
        [HttpPost(Routes.Profile.Create)]
        [Authorize(Roles = "Member", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create(CreateProfile command)
        {
            command.IdentityId = Request.HttpContext.User.Identities.First().Claims.FirstOrDefault(x => x.Type == "Id").Value;
            var response = await Mediator.Send(command);
            var resourceUrl = response.IsSuccess ? $"{HttpContext.Request.GetDisplayUrl()}/{response.Payload.ProfileId}" : string.Empty;
            return response.IsSuccess ? JsonResponse(response, resourceUrl) : JsonResponse(response);
        }

        [HttpPut(Routes.Profile.Update)]
        [Authorize(Roles = "Member", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromRoute] string ProfileId, UpdateProfile command)
        {
            command.ProfileId = ProfileId;
            command.IdentityId = Request.HttpContext.User.Identities.First().Claims.FirstOrDefault(x => x.Type == "Id").Value;
            var response = await Mediator.Send(command);
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