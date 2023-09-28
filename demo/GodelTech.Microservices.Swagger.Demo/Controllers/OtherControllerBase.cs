using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GodelTech.Microservices.Swagger.Demo.Controllers;

public class OtherControllerBase : ControllerBase
{
    [Authorize("add")]
    [SwaggerSecurity("fake.add")]
    [HttpGet("inherited/authorize")]
    public virtual IActionResult GetInheritedAuthorize()
    {
        return Ok();
    }
}
