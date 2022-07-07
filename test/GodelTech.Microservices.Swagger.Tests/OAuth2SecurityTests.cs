using Xunit;

namespace GodelTech.Microservices.Swagger.Tests;

public class OAuth2SecurityTests
{
    [Fact]
    public void OAuth2Security_Success()
    {
        // Arrange & Act & Assert
        Assert.Equal("oauth2", OAuth2Security.OAuth2);
        Assert.Equal("oauth2-authorization-code", OAuth2Security.AuthorizationCode);
        Assert.Equal("oauth2-client-credentials", OAuth2Security.ClientCredentials);
        Assert.Equal("oauth2-password", OAuth2Security.ResourceOwnerPasswordCredentials);
        Assert.Equal("oauth2-implicit", OAuth2Security.Implicit);
    }
}
