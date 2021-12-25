using CloudConsult.Common.Controllers;
using CloudConsult.Doctor.Domain.Commands;
using CloudConsult.Doctor.Domain.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Doctor.Api.Controllers;

[ApiVersion("1.0")]
[ApiController]
public class KycController : JsonController<KycController>
{
    [HttpPost(Routes.Kyc.Upload)]
    [Authorize(Roles = "Doctor", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Upload([FromRoute] string ProfileId, List<IFormFile> KycDocuments, CancellationToken cancellationToken = default)
    {
        var command = new UploadKycDocuments
        {
            ProfileId = ProfileId,
            KycDocuments = KycDocuments
        };
        var response = await Mediator.Send(command, cancellationToken);
        return JsonResponse(response);
    }

    [HttpGet(Routes.Kyc.DownloadAll)]
    [Authorize(Roles = "Doctor,Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DownloadAll([FromRoute] DownloadAllKyc query, CancellationToken cancellationToken = default)
    {
        var response = await Mediator.Send(query, cancellationToken);
        return JsonResponse(response);
    }

    [HttpGet(Routes.Kyc.DownloadOne)]
    [Authorize(Roles = "Doctor,Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DownloadOne([FromRoute] DownloadOneKyc query, CancellationToken cancellationToken = default)
    {
        var response = await Mediator.Send(query, cancellationToken);
        return JsonResponse(response);
    }

    [HttpPost(Routes.Kyc.Approve)]
    [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Approve([FromRoute] string ProfileId, ApproveKyc command, CancellationToken cancellationToken = default)
    {
        command.ProfileId = ProfileId;
        command.ApprovalIdentityId = Request.HttpContext.User.Identities.First().Claims.FirstOrDefault(x => x.Type == "Id").Value;
        var response = await Mediator.Send(command, cancellationToken);
        return JsonResponse(response);
    }

    [HttpPost(Routes.Kyc.Reject)]
    [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Reject([FromRoute] string ProfileId, RejectKyc command, CancellationToken cancellationToken = default)
    {
        command.ProfileId = ProfileId;
        command.RejectionIdentityId = Request.HttpContext.User.Identities.First().Claims.FirstOrDefault(x => x.Type == "Id").Value;
        var response = await Mediator.Send(command, cancellationToken);
        return JsonResponse(response);
    }

    [HttpGet(Routes.Kyc.GetMetadata)]
    public async Task<IActionResult> GetMetadata([FromRoute] string ProfileId, CancellationToken cancellationToken = default)
    {
        var query = new GetKycMetadata { ProfileId = ProfileId };
        var response = await Mediator.Send(query, cancellationToken);
        return JsonResponse(response);
    }
}
