using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FluentAssertions;
using GodelTech.Microservices.Swagger.Filters;
using GodelTech.Microservices.Swagger.Tests.Fakes;
using GodelTech.Microservices.Swagger.Utilities;
using Microsoft.OpenApi.Models;
using Moq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Xunit;

namespace GodelTech.Microservices.Swagger.Tests;

public class SwaggerInitializerTests
{
    private readonly Mock<IVersion> _mockVersion;

    public SwaggerInitializerTests()
    {
        _mockVersion = new Mock<IVersion>(MockBehavior.Strict);
    }

    public static IEnumerable<object[]> SwaggerGenOptionsMemberData =>
        new Collection<object[]>
        {
            new object[]
            {
                new Uri("http://testAuthorizationUrl.dev"),
                new Uri("http://testTokenUrl.dev"),
                new List<string>{
                    OAuth2Security.OAuth2,
                    OAuth2Security.AuthorizationCode,
                    OAuth2Security.ClientCredentials,
                    OAuth2Security.ResourceOwnerPasswordCredentials,
                    OAuth2Security.Implicit
                },
                5
            },
            new object[]
            {
                new Uri("http://testAuthorizationUrl.dev"),
                null,
                new List<string>{
                    OAuth2Security.OAuth2,
                    OAuth2Security.Implicit
                },
                2
            },
            new object[]
            {
                null,
                new Uri("http://testTokenUrl.dev"),
                new List<string>{
                    OAuth2Security.OAuth2,
                    OAuth2Security.ClientCredentials,
                },
                2
            },
            new object[]
            {
                null,
                null,
                new List<string>{
                    OAuth2Security.OAuth2,
                },
                1
            }
        };

    [Theory]
    [MemberData(nameof(SwaggerGenOptionsMemberData))]
    public void ConfigureSwaggerGenOptions_WhenXmlCommentsFilePathIsNull_Success(
        Uri authorizationUri,
        Uri tokenUri,
        IList<string> expectedSchemes,
        int expectedCount)
    {
        // Arrange
        var options = new SwaggerGenOptions();

        _mockVersion
            .Setup(x => x.GetVersion("Test DocumentVersion"))
            .Returns("1.2.3.4");

        var initializer = new FakeSwaggerInitializer(
            swaggerInitializerOptions =>
            {
                swaggerInitializerOptions.DocumentTitle = "Test DocumentTitle";
                swaggerInitializerOptions.DocumentVersion = "Test DocumentVersion";
                swaggerInitializerOptions.AuthorizationUrl = authorizationUri;
                swaggerInitializerOptions.TokenUrl = tokenUri;
            },
            _mockVersion.Object
        );

        var expectedOpeApiInfo = new OpenApiInfo
        {
            Title = "Test DocumentTitle",
            Version = "1.2.3.4"
        };

        // Act
        initializer.ExposedConfigureSwaggerGenOptions(options);

        // Assert
        Assert.Equal(
            expectedCount,
            options.SwaggerGeneratorOptions.SecuritySchemes.Keys.Count
        );
        Assert.True(options.SwaggerGeneratorOptions.SecuritySchemes.Keys.SequenceEqual(expectedSchemes));

        Assert.Single(options.SwaggerGeneratorOptions.SwaggerDocs);
        var swaggerDocs = Assert.Contains(
            "Test DocumentVersion",
            options.SwaggerGeneratorOptions.SwaggerDocs
        );
        swaggerDocs.Should().BeEquivalentTo(expectedOpeApiInfo);

        Assert.NotEmpty(options.DocumentFilterDescriptors);
        Assert.NotEmpty(options.ParameterFilterDescriptors);
        Assert.NotEmpty(options.RequestBodyFilterDescriptors);
        Assert.NotEmpty(options.OperationFilterDescriptors);
        Assert.NotEmpty(options.SchemaFilterDescriptors);

        var expectedOAuth2OperationFilter = options.OperationFilterDescriptors
            .FirstOrDefault(x => x.Type == typeof(OAuth2OperationFilter));
        Assert.NotNull(expectedOAuth2OperationFilter);

        var expectedIncludedXmlComments = options.DocumentFilterDescriptors
            .FirstOrDefault(x => x.Type == typeof(XmlCommentsDocumentFilter));
        Assert.Null(expectedIncludedXmlComments);
    }

    [Theory]
    [MemberData(nameof(SwaggerGenOptionsMemberData))]
    public void ConfigureSwaggerGenOptions_WhenXmlCommentsFilePathIsNotNull_Success(
        Uri authorizationUri,
        Uri tokenUri,
        IList<string> expectedSchemes,
        int expectedCount)
    {
        // Arrange
        var options = new SwaggerGenOptions();

        _mockVersion
            .Setup(x => x.GetVersion("Test DocumentVersion"))
            .Returns("1.2.3.4");

        var initializer = new FakeSwaggerInitializer(
            swaggerInitializerOptions =>
            {
                swaggerInitializerOptions.DocumentTitle = "Test DocumentTitle";
                swaggerInitializerOptions.DocumentVersion = "Test DocumentVersion";
                swaggerInitializerOptions.AuthorizationUrl = authorizationUri;
                swaggerInitializerOptions.TokenUrl = tokenUri;
                swaggerInitializerOptions.XmlCommentsFilePath = Path.GetFullPath("Documents/swagger.xml");
            },
            _mockVersion.Object
        );

        var expectedOpeApiInfo = new OpenApiInfo
        {
            Title = "Test DocumentTitle",
            Version = "1.2.3.4"
        };

        // Act
        initializer.ExposedConfigureSwaggerGenOptions(options);

        // Assert
        Assert.Equal(
            expectedCount,
            options.SwaggerGeneratorOptions.SecuritySchemes.Keys.Count
        );
        Assert.True(options.SwaggerGeneratorOptions.SecuritySchemes.Keys.SequenceEqual(expectedSchemes));

        Assert.Single(options.SwaggerGeneratorOptions.SwaggerDocs);
        var swaggerDocs = Assert.Contains(
            "Test DocumentVersion",
            options.SwaggerGeneratorOptions.SwaggerDocs
        );
        swaggerDocs.Should().BeEquivalentTo(expectedOpeApiInfo);

        Assert.NotEmpty(options.DocumentFilterDescriptors);
        Assert.NotEmpty(options.ParameterFilterDescriptors);
        Assert.NotEmpty(options.RequestBodyFilterDescriptors);
        Assert.NotEmpty(options.OperationFilterDescriptors);
        Assert.NotEmpty(options.SchemaFilterDescriptors);

        var expectedOAuth2OperationFilter = options.OperationFilterDescriptors
            .FirstOrDefault(x => x.Type == typeof(OAuth2OperationFilter));
        Assert.NotNull(expectedOAuth2OperationFilter);

        var expectedIncludedXmlComments = options.DocumentFilterDescriptors
            .FirstOrDefault(x => x.Type == typeof(XmlCommentsDocumentFilter));
        Assert.NotNull(expectedIncludedXmlComments);
    }

    [Fact]
    public void ConfigureCorrelationIdOptions_Success()
    {
        // Arrange
        var mockOptions = new Mock<SwaggerOptions>(MockBehavior.Strict);

        var initializer = new FakeSwaggerInitializer(null, _mockVersion.Object);

        // Act
        initializer.ExposedConfigureSwaggerOptions(mockOptions.Object);

        // Assert
        Assert.NotNull(mockOptions.Object);

        mockOptions.VerifyAll();
    }

    [Fact]
    public void ConfigureSwaggerUiOptions_Success()
    {
        // Arrange
        var options = new SwaggerUIOptions();

        var initializer = new FakeSwaggerInitializer(
            swaggerInitializerOptions =>
            {
                swaggerInitializerOptions.DocumentVersion = "Test DocumentVersion";
            },
            _mockVersion.Object
        );

        // Act
        initializer.ExposedConfigureSwaggerUiOptions(options);

        // Assert
        var swaggerEndpoint = Assert.Single(options.ConfigObject.Urls);

        Assert.Equal("/swagger/Test DocumentVersion/swagger.json", swaggerEndpoint.Url);
        Assert.Equal("Test DocumentVersion", swaggerEndpoint.Name);
    }
}
