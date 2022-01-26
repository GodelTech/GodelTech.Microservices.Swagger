using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GodelTech.Microservices.Swagger.Tests.Fakes
{
    [Authorize("add")]
    [SwaggerSecurity("Test scope")]
    public class FakeController : ControllerBase
    {
        public void FakeMethod(){}
    }
}