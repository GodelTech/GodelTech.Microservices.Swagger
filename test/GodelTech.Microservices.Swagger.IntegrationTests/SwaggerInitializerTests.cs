using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace GodelTech.Microservices.Swagger.IntegrationTests;

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

    [Theory]
    [InlineData("/swagger")]
    [InlineData("/swagger/index.html")]
    public async Task Configure_CheckHtml(string path)
    {
        // Arrange
        var client = _fixture.CreateClient();

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
        var client = _fixture.CreateClient();

        var expectedResultValue = await File.ReadAllTextAsync("Documents/swaggerJson.txt");
        expectedResultValue = expectedResultValue.Replace(
            Environment.NewLine,
            "\n",
            StringComparison.InvariantCulture
        );

        // Act
        var result = await client.GetAsync(
            new Uri(
                "/swagger/v2/swagger.json",
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
