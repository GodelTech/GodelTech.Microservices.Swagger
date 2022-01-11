using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using GodelTech.Microservices.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
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

        [Theory]
        [InlineData("/swagger")]
        [InlineData("/swagger/index.html")]
        public async Task Configure_CheckHtml(string path)
        {
            // Arrange
            var initializer = new SwaggerInitializer();

            var client = CreateClient(initializer);

            var expectedResultValue = await File.ReadAllTextAsync("Documents/swaggerHtml.txt");
            expectedResultValue = expectedResultValue.Replace(
                Environment.NewLine,
                "\n",
                StringComparison.InvariantCulture
            );

            // Act
            var result = await client.GetAsync(
                new Uri(
                    path,
                    UriKind.Relative
                )
            );

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(
                expectedResultValue,
                await result.Content.ReadAsStringAsync()
            );
        }

        [Fact]
        public async Task Configure_CheckJson()
        {
            // Arrange
            var initializer = new SwaggerInitializer();

            var client = CreateClient(initializer);

            var expectedResultValue = await File.ReadAllTextAsync("Documents/swaggerJson.txt");
            expectedResultValue = expectedResultValue.Replace(
                Environment.NewLine,
                "\n",
                StringComparison.InvariantCulture
            );

            // Act
            var result = await client.GetAsync(
                new Uri(
                    "/swagger/v1/swagger.json",
                    UriKind.Relative
                )
            );

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(
                expectedResultValue,
                await result.Content.ReadAsStringAsync()
            );
        }
    }
}