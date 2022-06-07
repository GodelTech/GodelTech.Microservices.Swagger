using System;
using GodelTech.Microservices.Swagger.Extensions;
using Microsoft.AspNetCore.Builder;
using Moq;
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

    [Fact]
    public void AddSwaggerRedirectHomePage_Success()
    {
        // Arrange
        var mockServiceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);

        var app = new ApplicationBuilder(mockServiceProvider.Object);

        // Act
        app.AddSwaggerRedirectHomePage();

        // Assert
    }
}