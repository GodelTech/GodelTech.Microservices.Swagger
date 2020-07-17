using System.Net;
using FluentAssertions;
using GodelTech.Microservices.Swagger.Tests.Utils;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace GodelTech.Microservices.Swagger.Tests
{
    [TestFixture]
    public class SwaggerInitializerTests : InMemoryServerTest
    {
        [Test]
        public void WhenDefaultConfigurationUser_Should_ReturnSwaggerUiAtDefaultAddress()
        {
            UseInMemoryServer(x => new[] { new SwaggerInitializer(x) }, new[] { typeof(RegressionV1Controller) });

            Client.GetAsync("/swagger/index.html").GetAwaiter().GetResult().StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public void WhenDefaultConfigurationUser_Should_ReturnSwaggerDocumentAtDefaultAddress()
        {
            UseInMemoryServer(x => new[] { new SwaggerInitializer(x) }, new[] { typeof(RegressionV1Controller) });

            Client.GetAsync("/swagger/v1/swagger.json").GetAwaiter().GetResult().StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Route("v1/users")]
        public class RegressionV1Controller : Controller
        {
            [HttpGet("{userId}")]
            public ActionResult GetByClientId(int userId)
            {
                return Ok();
            }
        }
    }
}
