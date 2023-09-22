using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Threading.Tasks;
using System;
using Xunit;

namespace GodelTech.Microservices.Swagger.IntegrationTests.Extensions;

public sealed class ApplicationBuilderExtensionsTests : IDisposable
{
    private readonly ExtensionsAppTestFixture _fixture;

    public ApplicationBuilderExtensionsTests()
    {
        _fixture = new ExtensionsAppTestFixture();
    }

    public void Dispose()
    {
        _fixture?.Dispose();
    }

    [Theory]
    [InlineData("")]
    [InlineData("/")]
    public async Task Configure_RedirectHomePage(string path)
    {
        // Arrange
        var client = _fixture.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("https://localhost:8080"),
                AllowAutoRedirect = false
            }
        );

        // Act
        var result = await client.GetAsync(
            new Uri(
                path,
                UriKind.Relative
            )
        );

        // Assert
        Assert.Equal(HttpStatusCode.MovedPermanently, result.StatusCode);
        Assert.Equal(
            "swagger",
            result.Headers.Location?.OriginalString
        );
    }
}
