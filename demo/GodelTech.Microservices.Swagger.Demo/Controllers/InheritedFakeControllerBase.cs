using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GodelTech.Microservices.Swagger.Demo.Controllers;

public abstract class InheritedFakeControllerBase : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("allowAnonymous")]
    public virtual IActionResult GetAllowAnonymous()
    {
        return Ok();
    }

    [Authorize]
    [HttpGet("override/allowAnonymous")]
    public virtual IActionResult GetOverrideAllowAnonymous()
    {
        return Ok();
    }

    [Authorize("add")]
    [HttpGet("authorize")]
    public virtual IActionResult GetAuthorize()
    {
        return Ok();
    }

    [Authorize("add")]
    [SwaggerSecurity("fake.add")]
    [HttpGet("authorizeWithSwaggerSecurity")]
    public virtual IActionResult GetAuthorizeWithSwaggerSecurity()
    {
        return Ok();
    }

    [SwaggerSecurity("fake.add")]
    [HttpGet("swaggerSecurity")]
    public virtual IActionResult GetSwaggerSecurity()
    {
        return Ok();
    }
}
