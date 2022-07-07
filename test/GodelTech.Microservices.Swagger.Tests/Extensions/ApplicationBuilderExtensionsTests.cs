using System;
using Xunit;

namespace GodelTech.Microservices.Swagger.Tests.Extensions;

public class ApplicationBuilderExtensionsTests
{
    [Fact]
    public void AddSwaggerRedirectHomePage_WhenApplicationBuilderIsNull_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => Swagger.Extensions.ApplicationBuilderExtensions.AddSwaggerRedirectHomePage(null, _ => { })
        );
        Assert.Equal("app", exception.ParamName);
    }
}
