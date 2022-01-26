using FluentAssertions;
using GodelTech.Microservices.Swagger.Filters;
using Microsoft.OpenApi.Models;
using Xunit;

namespace GodelTech.Microservices.Swagger.Tests.Filters
{
    public class OAuth2OperationFilterHelpersTests
    {
        [Fact]
        public void CreateOpenApiSecurityScheme_Success()
        {
            // Arrange
            var expectedOpenApiSecurityScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Test Id",
                    Type = ReferenceType.SecurityScheme
                }
            };

            // Act
            var result =
                OAuth2OperationFilterHelpers.CreateOpenApiSecurityScheme(
                    "Test Id");

            // Assert
            result.Should().BeEquivalentTo(expectedOpenApiSecurityScheme);
        }
    }
}