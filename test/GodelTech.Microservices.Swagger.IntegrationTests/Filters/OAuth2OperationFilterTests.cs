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

    [Theory]
    [InlineData("/fakes", HttpStatusCode.Unauthorized)]
    [InlineData("/fakes/1", HttpStatusCode.OK)]
    [InlineData("/others/authorize", HttpStatusCode.Unauthorized)]
    [InlineData("/others/inherited/authorize", HttpStatusCode.Unauthorized)]
    [InlineData("/inheritedFakes/allowAnonymous", HttpStatusCode.OK)]
    [InlineData("/inheritedFakes/override/allowAnonymous", HttpStatusCode.OK)]
    [InlineData("/inheritedFakes/authorize", HttpStatusCode.Unauthorized)]
    [InlineData("/inheritedFakes/authorizeWithSwaggerSecurity", HttpStatusCode.Unauthorized)]
    [InlineData("/inheritedFakes/swaggerSecurity", HttpStatusCode.Unauthorized)]
    public async Task Apply_WhenInheritedAttribute_AllowAnonymous(
        string path,
        HttpStatusCode expectedStatusCode)
    {
        // Arrange
        var client = _fixture.CreateClient();

        // Act
        var result = await client.GetAsync(
            new Uri(
                path,
                UriKind.Relative
            )
        );

        // Assert
        Assert.Equal(expectedStatusCode, result.StatusCode);
    }
}
