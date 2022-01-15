using FluentAssertions;
using GodelTech.Microservices.Swagger.Tests.Fakes;
using Swashbuckle.AspNetCore.SwaggerUI;
using Xunit;

namespace GodelTech.Microservices.Swagger.Tests
{
    public class SwaggerInitializerTests
    {
        private readonly FakeSwaggerInitializer _initializer;

        public SwaggerInitializerTests()
        {
            _initializer = new FakeSwaggerInitializer();
        }
        
        [Fact]
        public void ConfigureSwaggerUiOptions_Success()
        {
            // Arrange
            var options = new SwaggerUIOptions();

            // Act
            _initializer.ConfigureSwaggerUiOptions(options);

            // Assert
            var swaggerEndpoint = Assert.Single(options.ConfigObject.Urls);

            Assert.Equal("/swagger/v1/swagger.json", swaggerEndpoint.Url);
            Assert.Equal("v1", swaggerEndpoint.Name);
        }
    }
}