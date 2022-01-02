using Microsoft.AspNetCore.Mvc;

namespace CloudConsult.Notification.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}")]
public class DefaultController : ControllerBase
{
    private readonly ILogger<DefaultController> _logger;

    public DefaultController(ILogger<DefaultController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Notification service is up and running");
    }
}
