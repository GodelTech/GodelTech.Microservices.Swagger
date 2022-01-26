using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using FluentAssertions;
using GodelTech.Microservices.Swagger.Filters;
using GodelTech.Microservices.Swagger.Tests.Fakes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using Xunit;

namespace GodelTech.Microservices.Swagger.Tests
{
    public class SwaggerInitializerTests
    {
        public FakeSwaggerInitializer Initializer { get; set; }

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
        public void ConfigureSwaggerGenOptions_When_XmlCommentsFilePath_IsNull_Success(Uri authorizationUri, Uri tokenUri, IList<string> expectedSchemes, int expectedCount)
        {
            // Arrange
            var options = new SwaggerGenOptions();

            Initializer = new FakeSwaggerInitializer(
                swaggerInitializerOptions =>
                {
                    swaggerInitializerOptions.AuthorizationUrl = authorizationUri;
                    swaggerInitializerOptions.TokenUrl = tokenUri;
                }
            );
            var expectedOpeApiInfo = new OpenApiInfo
            {
                Title = "API",
                Version = "v1"
            };

            // Act
            Initializer.ExposedConfigureSwaggerGenOptions(options);

            // Assert
            Assert.Equal(expectedCount, options.SwaggerGeneratorOptions.SecuritySchemes.Keys.Count);
            Assert.True(options.SwaggerGeneratorOptions.SecuritySchemes.Keys.SequenceEqual(expectedSchemes));

            Assert.Single(options.SwaggerGeneratorOptions.SwaggerDocs);
            var swaggerDocs = Assert.Contains(
                "v1",
                options.SwaggerGeneratorOptions.SwaggerDocs
            );
            swaggerDocs.Should().BeEquivalentTo(expectedOpeApiInfo);

            Assert.NotEmpty(options.DocumentFilterDescriptors);
            Assert.NotEmpty(options.ParameterFilterDescriptors);
            Assert.NotEmpty(options.RequestBodyFilterDescriptors);
            Assert.NotEmpty(options.OperationFilterDescriptors);
            Assert.NotEmpty(options.SchemaFilterDescriptors);

            var expectedOAuth2OperationFilter = options.OperationFilterDescriptors
                .FirstOrDefault(
                    x =>
                        x.Type == typeof(OAuth2OperationFilter)
                );
            Assert.NotNull(expectedOAuth2OperationFilter);

            var expectedIncludedXmlComments = options.DocumentFilterDescriptors
                .FirstOrDefault(
                    x =>
                        x.Type == typeof(XmlCommentsDocumentFilter)
                );
            Assert.Null(expectedIncludedXmlComments);
        }

        [Theory]
        [MemberData(nameof(SwaggerGenOptionsMemberData))]
        public void ConfigureSwaggerGenOptions_When_XmlCommentsFilePath_IsNotNull_Success(Uri authorizationUri, Uri tokenUri, IList<string> expectedSchemes, int expectedCount)
        {
            // Arrange
            var options = new SwaggerGenOptions();
            
            Initializer = new FakeSwaggerInitializer(
                swaggerInitializerOptions =>
                {
                    swaggerInitializerOptions.AuthorizationUrl = authorizationUri;
                    swaggerInitializerOptions.TokenUrl = tokenUri;
                    swaggerInitializerOptions.XmlCommentsFilePath = Path.GetFullPath("Documents/swagger.xml");
                }
            );
            var expectedOpeApiInfo = new OpenApiInfo
            {
                Title = "API",
                Version = "v1"
            };

            // Act
            Initializer.ExposedConfigureSwaggerGenOptions(options);

            // Assert
            Assert.Equal(
                expectedCount,
                options.SwaggerGeneratorOptions.SecuritySchemes.Keys.Count
            );
            Assert.True(options.SwaggerGeneratorOptions.SecuritySchemes.Keys.SequenceEqual(expectedSchemes));

            Assert.Single(options.SwaggerGeneratorOptions.SwaggerDocs);
            var swaggerDocs = Assert.Contains(
                "v1",
                options.SwaggerGeneratorOptions.SwaggerDocs
            );
            swaggerDocs.Should().BeEquivalentTo(expectedOpeApiInfo);

            Assert.NotEmpty(options.DocumentFilterDescriptors);
            Assert.NotEmpty(options.ParameterFilterDescriptors);
            Assert.NotEmpty(options.RequestBodyFilterDescriptors);
            Assert.NotEmpty(options.OperationFilterDescriptors);
            Assert.NotEmpty(options.SchemaFilterDescriptors);

            var expectedOAuth2OperationFilter = options.OperationFilterDescriptors
                .FirstOrDefault(
                    x =>
                        x.Type == typeof(OAuth2OperationFilter)
                );
            Assert.NotNull(expectedOAuth2OperationFilter);

            var expectedIncludedXmlComments = options.DocumentFilterDescriptors
                .FirstOrDefault(
                    x =>
                        x.Type == typeof(XmlCommentsDocumentFilter)
                );
            Assert.NotNull(expectedIncludedXmlComments);
        }

        [Fact]
        public void ConfigureSwaggerUiOptions_Success()
        {
            // Arrange
            var options = new SwaggerUIOptions();

            // Act
            Initializer = new FakeSwaggerInitializer(null);
            Initializer.ExposedConfigureSwaggerUiOptions(options);

            // Assert
            var swaggerEndpoint = Assert.Single(options.ConfigObject.Urls);

            Assert.Equal("/swagger/v1/swagger.json", swaggerEndpoint.Url);
            Assert.Equal("v1", swaggerEndpoint.Name);
        }
    }
}