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

    [HttpGet(Routes.Kyc.Download)]
    public async Task<IActionResult> Download([FromRoute] DownloadKycDocuments query, CancellationToken cancellationToken = default)
    {
        var response = await Mediator.Send(query, cancellationToken);
        if (response.IsSuccess)
        {
            return File(response.Payload.ArchiveData, response.Payload.FileType, response.Payload.ArchiveName);
        }
        return JsonResponse(response);
    }

    [HttpPost(Routes.Kyc.Approve)]
    [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Approve([FromRoute] string ProfileId, ApproveKyc command, CancellationToken cancellationToken = default)
    {
        command.ProfileId = ProfileId;
        var response = await Mediator.Send(command, cancellationToken);
        return JsonResponse(response);
    }

    [HttpPost(Routes.Kyc.Reject)]
    [Authorize(Roles = "Administrator", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Reject([FromRoute] string ProfileId, RejectKyc command, CancellationToken cancellationToken = default)
    {
        command.ProfileId = ProfileId;
        var response = await Mediator.Send(command, cancellationToken);
        return JsonResponse(response);
    }
}
