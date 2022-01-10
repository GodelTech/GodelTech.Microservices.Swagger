//using System;
//using System.Net;
//using FluentAssertions;
//using GodelTech.Microservices.Core;
//using GodelTech.Microservices.Swagger.Configuration;
//using GodelTech.Microservices.Swagger.Tests.Utils;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using NUnit.Framework;

//namespace GodelTech.Microservices.Swagger.Tests
//{
//    [TestFixture]
//    public class SwaggerInitializerTests : InMemoryServerTest
//    {
//        [Test]
//        public void WhenDefaultConfiguration_Should_ReturnSwaggerUiAtDefaultAddress()
//        {
//            SetUpInMemoryServer();

//            Client.GetAsync("/swagger/index.html").GetAwaiter().GetResult().StatusCode.Should().Be(HttpStatusCode.OK);
//        }

//        [Test]
//        public void WhenDefaultConfiguration_Should_ReturnSwaggerDocumentAtDefaultAddress()
//        {
//            SetUpInMemoryServer();

//            Client.GetAsync("/swagger/v1/swagger.json").GetAwaiter().GetResult().StatusCode.Should().Be(HttpStatusCode.OK);
//        }

//        [Test]
//        public void WhenApiVersionIsNonDefault_Should_ReturnSwaggerDocumentAtAddressContainingApiVersion()
//        {
//            SetUpInMemoryServer(x =>
//            {
//                x.DocumentVersion = "v123";
//            });

//            Client.GetAsync("/swagger/v123/swagger.json").GetAwaiter().GetResult().StatusCode.Should().Be(HttpStatusCode.OK);
//        }

//        private void SetUpInMemoryServer(Action<SwaggerInitializerOptions> configure = null)
//        {
//            UseInMemoryServer(x =>
//                    new[]
//                    {
//                        CreateSwaggerInitializer(x, configure)
//                    },
//                    new[]
//                    {
//                        typeof(RegressionV1Controller)
//                    });
//        }

//        private IMicroserviceInitializer CreateSwaggerInitializer(IConfiguration configuration, Action<SwaggerInitializerOptions> configure)
//        {
//            var initializer = new SwaggerInitializer(configuration);

//            configure?.Invoke(initializer.Options);

//            return initializer;
//        }

//        #region Helper classes

//        [Route("v1/users")]
//        public class RegressionV1Controller : Controller
//        {
//            [HttpGet("{userId}")]
//            public ActionResult GetByClientId(int userId)
//            {
//                return Ok();
//            }
//        }

//        #endregion
//    }
//}
