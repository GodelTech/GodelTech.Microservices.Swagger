using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GodelTech.Microservices.Swagger.Demo.Controllers;

[Route("others")]
[ApiController]
public class OtherController : OtherControllerBase
{
    [Authorize]
    [HttpGet("authorize")]
    public IActionResult GetAuthorize()
    {
        return Ok();
    }

    public override IActionResult GetInheritedAuthorize()
    {
        return Ok();
    }
}
