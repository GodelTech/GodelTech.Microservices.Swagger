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

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using GodelTech.Microservices.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GodelTech.Microservices.Swagger.IntegrationTests
{
    public sealed class SwaggerInitializerTests : IDisposable
    {
        private readonly AppTestFixture _fixture;

        public SwaggerInitializerTests()
        {
            _fixture = new AppTestFixture();
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        private HttpClient CreateClient(IMicroserviceInitializer initializer)
        {
            return _fixture
                .WithWebHostBuilder(
                    builder =>
                    {
                        builder
                            .ConfigureServices(
                                services =>
                                {
                                    initializer.ConfigureServices(services);

                                    services.AddControllers();
                                }
                            );

                        builder
                            .Configure(
                                (context, app) =>
                                {
                                    initializer.Configure(app, context.HostingEnvironment);

                                    app.UseRouting();

                                    app.UseEndpoints(
                                        endpoints =>
                                        {
                                            endpoints.MapControllers();
                                        }
                                    );
                                }
                            );
                    }
                )
                .CreateClient();
        }

        [Fact]
        public async Task Configure_Success()
        {
            // Arrange
            var initializer = new SwaggerInitializer();

            var client = CreateClient(initializer);

            // Act
            // Result	"{\n  \"openapi\": \"3.0.1\",\n  \"info\": {\n    \"title\": \"API\",\n    \"version\": \"v1\"\n  },\n  \"paths\": {\n    \"/fakes\": {\n      \"get\": {\n        \"tags\": [\n          \"Fake\"\n        ],\n        \"responses\": {\n          \"200\": {\n            \"description\": \"Success\",\n            \"content\": {\n              \"text/plain\": {\n                \"schema\": {\n                  \"type\": \"array\",\n                  \"items\": {\n                    \"$ref\": \"#/components/schemas/FakeModel\"\n                  }\n                }\n              },\n              \"application/json\": {\n                \"schema\": {\n                  \"type\": \"array\",\n                  \"items\": {\n                    \"$ref\": \"#/components/schemas/FakeModel\"\n                  }\n                }\n              },\n              \"text/json\": {\n                \"schema\": {\n                  \"type\": \"array\",\n                  \"items\": {\n                    \"$ref\": \"#/components/schemas/FakeModel\"\n                  }\n                }\n              }\n            }\n          },\n          \"401\": {\n            \"description\": \"Unauthorized\"\n          },\n          \"403\": {\n            \"description\": \"Forbidden\"\n          }\n        },\n        \"security\": [\n          {\n            \"oauth2\": [ ],\n            \"oauth2-authorization-code\": [ ],\n            \"oauth2-implicit\": [ ],\n            \"oauth2-client-credentials\": [ ],\n            \"oauth2-password\": [ ]\n          }\n        ]\n      },\n      \"post\": {\n        \"tags\": [\n          \"Fake\"\n        ],\n        \"requestBody\": {\n          \"content\": {\n            \"application/json\": {\n              \"schema\": {\n                \"$ref\": \"#/components/schemas/FakePostModel\"\n              }\n            },\n            \"text/json\": {\n              \"schema\": {\n                \"$ref\": \"#/components/schemas/FakePostModel\"\n              }\n            },\n            \"application/*+json\": {\n              \"schema\": {\n                \"$ref\": \"#/components/schemas/FakePostModel\"\n              }\n            }\n          }\n        },\n        \"responses\": {\n          \"201\": {\n            \"description\": \"Success\",\n            \"content\": {\n              \"text/plain\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/FakeModel\"\n                }\n              },\n              \"application/json\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/FakeModel\"\n                }\n              },\n              \"text/json\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/FakeModel\"\n                }\n              }\n            }\n          },\n          \"401\": {\n            \"description\": \"Unauthorized\"\n          },\n          \"403\": {\n            \"description\": \"Forbidden\"\n          }\n        },\n        \"security\": [\n          {\n            \"oauth2\": [ ],\n            \"oauth2-authorization-code\": [ ],\n            \"oauth2-implicit\": [ ],\n            \"oauth2-client-credentials\": [ ],\n            \"oauth2-password\": [ ]\n          }\n        ]\n      }\n    },\n    \"/fakes/{id}\": {\n      \"get\": {\n        \"tags\": [\n          \"Fake\"\n        ],\n        \"parameters\": [\n          {\n            \"name\": \"id\",\n            \"in\": \"path\",\n            \"required\": true,\n            \"schema\": {\n              \"type\": \"integer\",\n              \"format\": \"int32\"\n            }\n          }\n        ],\n        \"responses\": {\n          \"404\": {\n            \"description\": \"Not Found\",\n            \"content\": {\n              \"text/plain\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/ProblemDetails\"\n                }\n              },\n              \"application/json\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/ProblemDetails\"\n                }\n              },\n              \"text/json\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/ProblemDetails\"\n                }\n              }\n            }\n          },\n          \"200\": {\n            \"description\": \"Success\",\n            \"content\": {\n              \"text/plain\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/FakeModel\"\n                }\n              },\n              \"application/json\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/FakeModel\"\n                }\n              },\n              \"text/json\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/FakeModel\"\n                }\n              }\n            }\n          },\n          \"401\": {\n            \"description\": \"Unauthorized\"\n          },\n          \"403\": {\n            \"description\": \"Forbidden\"\n          }\n        },\n        \"security\": [\n          {\n            \"oauth2\": [ ],\n            \"oauth2-authorization-code\": [ ],\n            \"oauth2-implicit\": [ ],\n            \"oauth2-client-credentials\": [ ],\n            \"oauth2-password\": [ ]\n          }\n        ]\n      },\n      \"put\": {\n        \"tags\": [\n          \"Fake\"\n        ],\n        \"parameters\": [\n          {\n            \"name\": \"id\",\n            \"in\": \"path\",\n            \"required\": true,\n            \"schema\": {\n              \"type\": \"integer\",\n              \"format\": \"int32\"\n            }\n          }\n        ],\n        \"requestBody\": {\n          \"content\": {\n            \"application/json\": {\n              \"schema\": {\n                \"$ref\": \"#/components/schemas/FakePutModel\"\n              }\n            },\n            \"text/json\": {\n              \"schema\": {\n                \"$ref\": \"#/components/schemas/FakePutModel\"\n              }\n            },\n            \"application/*+json\": {\n              \"schema\": {\n                \"$ref\": \"#/components/schemas/FakePutModel\"\n              }\n            }\n          }\n        },\n        \"responses\": {\n          \"400\": {\n            \"description\": \"Bad Request\",\n            \"content\": {\n              \"text/plain\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/ProblemDetails\"\n                }\n              },\n              \"application/json\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/ProblemDetails\"\n                }\n              },\n              \"text/json\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/ProblemDetails\"\n                }\n              }\n            }\n          },\n          \"404\": {\n            \"description\": \"Not Found\",\n            \"content\": {\n              \"text/plain\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/ProblemDetails\"\n                }\n              },\n              \"application/json\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/ProblemDetails\"\n                }\n              },\n              \"text/json\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/ProblemDetails\"\n                }\n              }\n            }\n          },\n          \"204\": {\n            \"description\": \"Success\"\n          },\n          \"401\": {\n            \"description\": \"Unauthorized\"\n          },\n          \"403\": {\n            \"description\": \"Forbidden\"\n          }\n        },\n        \"security\": [\n          {\n            \"oauth2\": [ ],\n            \"oauth2-authorization-code\": [ ],\n            \"oauth2-implicit\": [ ],\n            \"oauth2-client-credentials\": [ ],\n            \"oauth2-password\": [ ]\n          }\n        ]\n      },\n      \"delete\": {\n        \"tags\": [\n          \"Fake\"\n        ],\n        \"parameters\": [\n          {\n            \"name\": \"id\",\n            \"in\": \"path\",\n            \"required\": true,\n            \"schema\": {\n              \"type\": \"integer\",\n              \"format\": \"int32\"\n            }\n          }\n        ],\n        \"responses\": {\n          \"404\": {\n            \"description\": \"Not Found\",\n            \"content\": {\n              \"text/plain\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/ProblemDetails\"\n                }\n              },\n              \"application/json\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/ProblemDetails\"\n                }\n              },\n              \"text/json\": {\n                \"schema\": {\n                  \"$ref\": \"#/components/schemas/ProblemDetails\"\n                }\n              }\n            }\n          },\n          \"200\": {\n            \"description\": \"Success\"\n          },\n          \"401\": {\n            \"description\": \"Unauthorized\"\n          },\n          \"403\": {\n            \"description\": \"Forbidden\"\n          }\n        },\n        \"security\": [\n          {\n            \"oauth2\": [ ],\n            \"oauth2-authorization-code\": [ ],\n            \"oauth2-implicit\": [ ],\n            \"oauth2-client-credentials\": [ ],\n            \"oauth2-password\": [ ]\n          }\n        ]\n      }\n    }\n  },\n  \"components\": {\n    \"schemas\": {\n      \"FakeModel\": {\n        \"type\": \"object\",\n        \"properties\": {\n          \"id\": {\n            \"type\": \"integer\",\n            \"format\": \"int32\"\n          },\n          \"title\": {\n            \"type\": \"string\",\n            \"nullable\": true\n          }\n        },\n        \"additionalProperties\": false\n      },\n      \"FakePostModel\": {\n        \"type\": \"object\",\n        \"properties\": {\n          \"title\": {\n            \"type\": \"string\",\n            \"nullable\": true\n          }\n        },\n        \"additionalProperties\": false\n      },\n      \"FakePutModel\": {\n        \"type\": \"object\",\n        \"properties\": {\n          \"id\": {\n            \"type\": \"integer\",\n            \"format\": \"int32\"\n          },\n          \"title\": {\n            \"type\": \"string\",\n            \"nullable\": true\n          }\n        },\n        \"additionalProperties\": false\n      },\n      \"ProblemDetails\": {\n        \"type\": \"object\",\n        \"properties\": {\n          \"type\": {\n            \"type\": \"string\",\n            \"nullable\": true\n          },\n          \"title\": {\n            \"type\": \"string\",\n            \"nullable\": true\n          },\n          \"status\": {\n            \"type\": \"integer\",\n            \"format\": \"int32\",\n            \"nullable\": true\n          },\n          \"detail\": {\n            \"type\": \"string\",\n            \"nullable\": true\n          },\n          \"instance\": {\n            \"type\": \"string\",\n            \"nullable\": true\n          }\n        },\n        \"additionalProperties\": { }\n      }\n    },\n    \"securitySchemes\": {\n      \"oauth2\": {\n        \"type\": \"apiKey\",\n        \"description\": \"Standard Authorization header using the Bearer scheme. Example: \\\"bearer {token}\\\"\",\n        \"name\": \"Authorization\",\n        \"in\": \"header\"\n      }\n    }\n  }\n}"	string

            var result = await client.GetAsync(
                new Uri(
                    "/swagger/v1/swagger.json",
                    UriKind.Relative
                )
            );

            var actualSwaggerDoc = JObject.Parse(await result.Content.ReadAsStringAsync());
            var expectedSwaggerDoc = JObject.Parse(
                await File.ReadAllTextAsync("expectedSwaggerDoc.json")
            );

            // Assert
            Assert.True(JToken.DeepEquals(actualSwaggerDoc, expectedSwaggerDoc));
            /*Assert.Matches(
                new Regex(
                          await File.ReadAllTextAsync("expectedSwaggerDoc.json"), RegexOptions.Multiline | RegexOptions.Singleline),
                    actualSwaggerDoc
            );*/

        }
    }
}