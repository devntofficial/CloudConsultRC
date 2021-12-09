using CloudConsult.Common.Builders;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CloudConsult.Common.Controllers;

public class JsonController<T> : ControllerBase where T : JsonController<T>
{
    private ILogger<T> _logger;
    protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();

    private IMediator _mediator;
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected IActionResult JsonResponse<TResponse>(IApiResponse<TResponse> response, string resourceUrl = null,
        string redirectUrl = null)
        => (response.StatusCode, resourceUrl, redirectUrl) switch
        {
            (StatusCodes.Status200OK, _, _) => Ok(response),
            (StatusCodes.Status201Created, _, _) => Created(resourceUrl, response),
            (StatusCodes.Status202Accepted, _, _) => Accepted(response),
            (StatusCodes.Status204NoContent, _, _) => NoContent(),
            (StatusCodes.Status301MovedPermanently, _, _) => LocalRedirectPermanent(redirectUrl),
            (StatusCodes.Status302Found, _, _) => LocalRedirect(redirectUrl),
            (StatusCodes.Status307TemporaryRedirect, _, _) => LocalRedirectPreserveMethod(redirectUrl),
            (StatusCodes.Status308PermanentRedirect, _, _) => LocalRedirectPermanentPreserveMethod(redirectUrl),
            (StatusCodes.Status400BadRequest, _, _) => BadRequest(response),
            (StatusCodes.Status401Unauthorized, _, _) => Unauthorized(response),
            (StatusCodes.Status404NotFound, _, _) => NotFound(response),
            (StatusCodes.Status409Conflict, _, _) => Conflict(response),
            (_) => BadRequest(response)
        };

    protected IActionResult JsonResponse(IApiResponse response, string resourceUrl = null,
        string redirectUrl = null)
        => (response.StatusCode, resourceUrl, redirectUrl) switch
        {
            (StatusCodes.Status200OK, _, _) => Ok(response),
            (StatusCodes.Status201Created, _, _) => Created(resourceUrl, response),
            (StatusCodes.Status202Accepted, _, _) => Accepted(response),
            (StatusCodes.Status204NoContent, _, _) => NoContent(),
            (StatusCodes.Status301MovedPermanently, _, _) => LocalRedirectPermanent(redirectUrl),
            (StatusCodes.Status302Found, _, _) => LocalRedirect(redirectUrl),
            (StatusCodes.Status307TemporaryRedirect, _, _) => LocalRedirectPreserveMethod(redirectUrl),
            (StatusCodes.Status308PermanentRedirect, _, _) => LocalRedirectPermanentPreserveMethod(redirectUrl),
            (StatusCodes.Status400BadRequest, _, _) => BadRequest(response),
            (StatusCodes.Status401Unauthorized, _, _) => Unauthorized(response),
            (StatusCodes.Status404NotFound, _, _) => NotFound(response),
            (StatusCodes.Status409Conflict, _, _) => Conflict(response),
            (_) => BadRequest(response)
        };
}
