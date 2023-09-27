using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GodelTech.Microservices.Swagger.Demo.Controllers;

[Route("inheritedFakes")]
[ApiController]
public class InheritedFakeController : InheritedFakeControllerBase
{
    public override IActionResult GetAllowAnonymous()
    {
        return Ok();
    }

    [AllowAnonymous]
    public override IActionResult GetOverrideAllowAnonymous()
    {
        return Ok();
    }

    public override IActionResult GetAuthorize()
    {
        return Ok();
    }

    public override IActionResult GetAuthorizeWithSwaggerSecurity()
    {
        return Ok();
    }

    public override IActionResult GetSwaggerSecurity()
    {
        return Ok();
    }
}
