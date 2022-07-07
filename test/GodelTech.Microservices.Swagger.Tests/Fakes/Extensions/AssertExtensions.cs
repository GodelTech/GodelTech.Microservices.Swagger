using System;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace GodelTech.Microservices.Swagger.Tests.Fakes.Extensions;

public static class AssertExtensions
{
    public static void ArgumentNullException(
        Action<SwaggerGenOptions> action,
        string expectedParamName = "initializerOptions")
    {
        // Arrange 
        var options = new SwaggerGenOptions();

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(
            () => action(options)
        );
        Assert.Equal(expectedParamName, exception.ParamName);
    }

    public static void ArgumentException(
        SwaggerInitializerOptions initializerOptions,
        Action<SwaggerGenOptions, SwaggerInitializerOptions> action,
        string expectedMessage)
    {
        // Arrange 
        var options = new SwaggerGenOptions();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(
            () => action(options, initializerOptions)
        );
        Assert.Equal(expectedMessage, exception.Message);
    }
}
