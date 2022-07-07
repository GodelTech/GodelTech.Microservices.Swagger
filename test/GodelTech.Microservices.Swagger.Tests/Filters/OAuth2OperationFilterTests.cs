using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text.Json;
using FluentAssertions;
using GodelTech.Microservices.Swagger.Demo.Controllers;
using GodelTech.Microservices.Swagger.Filters;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace GodelTech.Microservices.Swagger.Tests.Filters;

public class OAuth2OperationFilterTests
{
    private readonly OAuth2OperationFilter _filter;

    public OAuth2OperationFilterTests()
    {
        _filter = new OAuth2OperationFilter();
    }

    [Fact]
    public void Apply_WhenOpenApiOperationIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var context = new OperationFilterContext(
            new ApiDescription(),
            new SchemaGenerator(
                new SchemaGeneratorOptions(),
                new JsonSerializerDataContractResolver(
                    new JsonSerializerOptions()
                )
            ),
            new SchemaRepository(),
            new DynamicMethod("Test", null, null)
        );

        // Act & Assert
        var result = Assert.Throws<ArgumentNullException>(
            () => _filter.Apply(null, context)
        );

        Assert.Equal("operation", result.ParamName);
    }

    [Fact]
    public void Apply_WhenOperationFilterContextIsNull_ThrowsArgumentNullException()
    {
        // Arrange & Act
        var result = Assert.Throws<ArgumentNullException>(
            () => _filter.Apply(new OpenApiOperation(), null)
        );

        // Assert
        Assert.Equal("context", result.ParamName);
    }

    [Fact]
    public void Apply_WhenAuthorizeAttributesOrSwaggerSecurityAttributesIsEmpty_Success()
    {
        // Arrange
        var context = new OperationFilterContext(
            new ApiDescription(),
            new SchemaGenerator(
                new SchemaGeneratorOptions(),
                new JsonSerializerDataContractResolver(
                    new JsonSerializerOptions()
                )
            ),
            new SchemaRepository(),
            new DynamicMethod("Test Name", null, null)
        );

        var operation = new OpenApiOperation();

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.Empty(operation.Responses);
        Assert.Empty(operation.Security);
    }

    [Fact]
    public void Apply_Success()
    {
        // Arrange
        var method = typeof(FakeController).GetMethod("Post");
        var scope = new List<string> { "fake.add" };

        var context = new OperationFilterContext(
            new ApiDescription(),
            new SchemaGenerator(
                new SchemaGeneratorOptions(),
                new JsonSerializerDataContractResolver(
                    new JsonSerializerOptions()
                )
            ),
            new SchemaRepository(),
            method
        );

        var operation = new OpenApiOperation
        {
            Security = null
        };

        var expectedOpenApiResponses = new Dictionary<string, OpenApiResponse>
        {
            {"401", new OpenApiResponse {Description = "Unauthorized"}},
            {"403", new OpenApiResponse {Description = "Forbidden"}}
        };

        var expectedOpenApiSecurityRequirements = new List<OpenApiSecurityRequirement>
        {
            new OpenApiSecurityRequirement
            {
                {
                    OAuth2OperationFilterHelpers.CreateOpenApiSecurityScheme(OAuth2Security.OAuth2),
                    scope
                },
                {
                    OAuth2OperationFilterHelpers.CreateOpenApiSecurityScheme(OAuth2Security.AuthorizationCode),
                    scope
                },
                {
                    OAuth2OperationFilterHelpers.CreateOpenApiSecurityScheme(OAuth2Security.ClientCredentials),
                    scope
                },
                {
                    OAuth2OperationFilterHelpers.CreateOpenApiSecurityScheme(OAuth2Security.ResourceOwnerPasswordCredentials),
                    scope
                },
                {
                    OAuth2OperationFilterHelpers.CreateOpenApiSecurityScheme(OAuth2Security.Implicit),
                    scope
                }
            }
        };

        // Act
        _filter.Apply(operation, context);

        // Assert
        Assert.Equal(expectedOpenApiResponses.Count, operation.Responses.Count);

        operation.Responses
            .Should()
            .BeEquivalentTo(expectedOpenApiResponses);

        Assert.Equal(expectedOpenApiSecurityRequirements.Count, operation.Security?.Count);

        operation.Security
            .Should()
            .BeEquivalentTo(expectedOpenApiSecurityRequirements);
    }
}
