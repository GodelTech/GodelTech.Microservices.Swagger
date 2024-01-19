using System.Reflection;
using GodelTech.Microservices.Swagger.Utilities;
using Xunit;

namespace GodelTech.Microservices.Swagger.Tests.Utilities;

public class SystemVersionTests
{
    [Fact]
    public void GetVersion_Success()
    {
        // Arrange
        var utility = new SystemVersion();

        // Act
        var result = utility.GetVersion("1.2.3.4");

        // Assert
        Assert.Equal(Assembly.GetEntryAssembly()?.GetName().Version?.ToString(), result);
    }

    [Fact]
    public void GetVersion_WhenAssemblyIsNull_ReturnsDefaultVersion()
    {
        // Arrange
        var utility = new SystemVersion(() => null);

        // Act
        var result = utility.GetVersion("1.2.3.4");

        // Assert
        Assert.Equal("1.2.3.4", result);
    }
}
