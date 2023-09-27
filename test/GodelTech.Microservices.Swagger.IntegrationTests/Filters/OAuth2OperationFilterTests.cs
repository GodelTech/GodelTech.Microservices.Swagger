using System.Net;
using System.Threading.Tasks;
using System;
using Xunit;

namespace GodelTech.Microservices.Swagger.IntegrationTests.Filters;

public sealed class OAuth2OperationFilterTests : IDisposable
{
    private readonly AppTestFixture _fixture;

    public OAuth2OperationFilterTests()
    {
        _fixture = new AppTestFixture();
    }

    public void Dispose()
    {
        _fixture?.Dispose();
    }

    [Fact]
    public async Task Apply_WhenInheritedAttribute_AllowAnonymous()
    {
        // Arrange
        var client = _fixture.CreateClient();

        // Act
        var result = await client.GetAsync(
            new Uri(
                "/inheritedFakes/allowAnonymous",
                UriKind.Relative
            )
        );

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task Apply_WhenInheritedAttribute_OverrideAllowAnonymous()
    {
        // Arrange
        var client = _fixture.CreateClient();

        // Act
        var result = await client.GetAsync(
            new Uri(
                "/inheritedFakes/override/allowAnonymous",
                UriKind.Relative
            )
        );

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task Apply_WhenInheritedAttribute_Authorize()
    {
        // Arrange
        var client = _fixture.CreateClient();

        // Act
        var result = await client.GetAsync(
            new Uri(
                "/inheritedFakes/authorize",
                UriKind.Relative
            )
        );

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task Apply_WhenInheritedAttribute_AuthorizeWithSwaggerSecurity()
    {
        // Arrange
        var client = _fixture.CreateClient();

        // Act
        var result = await client.GetAsync(
            new Uri(
                "/inheritedFakes/authorizeWithSwaggerSecurity",
                UriKind.Relative
            )
        );

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);
    }

    [Fact]
    public async Task Apply_WhenInheritedAttribute_SwaggerSecurity()
    {
        // Arrange
        var client = _fixture.CreateClient();

        // Act
        var result = await client.GetAsync(
            new Uri(
                "/inheritedFakes/swaggerSecurity",
                UriKind.Relative
            )
        );

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
}
