using System;
using System.Collections.Generic;
using FluentAssertions;
using GodelTech.Microservices.Swagger.Extensions;
using GodelTech.Microservices.Swagger.Swagger;
using GodelTech.Microservices.Swagger.Tests.Fakes;
using GodelTech.Microservices.Swagger.Tests.Fakes.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace GodelTech.Microservices.Swagger.Tests.Extensions
{
    public class SwaggerGenOptionsExtensionsTests
    {
        private const string AuthorizationUrlArgumentExceptionMessage = "AuthorizationUrl can't be null";
        private const string TokenUrlArgumentExceptionMessage = "TokenUrl can't be null";
        private const string ScopesArgumentExceptionMessage = "Scopes can't be null";

        [Fact]
        public void AddAuthHeaderFlowSecurityDefinition_Success()
        {
            // Arrange
            var options = new SwaggerGenOptions();

            var expectedOpenApiSecurityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\""
            };

            // Act
            options.AddAuthHeaderFlowSecurityDefinition();

            // Assert
            Assert.Single(options.SwaggerGeneratorOptions.SecuritySchemes);

            var securityScheme = Assert.Contains(
                OAuth2Security.OAuth2,
                options.SwaggerGeneratorOptions.SecuritySchemes
            );

            securityScheme.Should().BeEquivalentTo(expectedOpenApiSecurityScheme);
        }

        [Fact]
        public void AddAuthorizationCodeFlowSecurityDefinition_WhenSwaggerInitializerOptionsIsNull_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentNullException(
                options => options.AddAuthorizationCodeFlowSecurityDefinition(null)
            );
        }

        [Fact]
        public void AddAuthorizationCodeFlowSecurityDefinition_WhenAuthorizationUrlIsNull_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentException(
                SwaggerInitializerOptionsHelpers.CreateWithNullAuthorizationUrl(),
                (options, swaggerInitializerOptions) => options.AddAuthorizationCodeFlowSecurityDefinition(swaggerInitializerOptions),
                AuthorizationUrlArgumentExceptionMessage
            );
        }

        [Fact]
        public void
            AddAuthorizationCodeFlowSecurityDefinition_WhenTokenUrlIsNull_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentException(
                SwaggerInitializerOptionsHelpers.CreateWithNullTokenUrl(),
                (options, swaggerInitializerOptions) => options.AddAuthorizationCodeFlowSecurityDefinition(swaggerInitializerOptions),
                TokenUrlArgumentExceptionMessage
            );
        }

        [Fact]
        public void AddAuthorizationCodeFlowSecurityDefinition_WhenScopesIsNull_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentException(
                SwaggerInitializerOptionsHelpers.CreateWithNullScopes(),
                (options, swaggerInitializerOptions) => options.AddAuthorizationCodeFlowSecurityDefinition(swaggerInitializerOptions),
                ScopesArgumentExceptionMessage
            );
        }

        [Fact]
        public void AddAuthorizationCodeFlowSecurityDefinition_Success()
        {
            // Arrange
            var options = new SwaggerGenOptions();
            var initializerOptions = new SwaggerInitializerOptions
            {
                AuthorizationUrl = new Uri("http://test.dev"),
                TokenUrl = new Uri("http://test.dev"),
                Scopes = new Dictionary<string, string>
                {
                    {"TestScopeKey", "TestScopeValue"}
                }
            };

            var expectedOpenApiSecurityScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = initializerOptions.AuthorizationUrl,
                        TokenUrl = initializerOptions.TokenUrl,
                        Scopes = initializerOptions.Scopes
                    }
                }
            };

            // Act
            options.AddAuthorizationCodeFlowSecurityDefinition(initializerOptions);

            // Assert
            Assert.Single(options.SwaggerGeneratorOptions.SecuritySchemes);

            var securityScheme = Assert.Contains(
                OAuth2Security.AuthorizationCode,
                options.SwaggerGeneratorOptions.SecuritySchemes
            );

            securityScheme.Should().BeEquivalentTo(expectedOpenApiSecurityScheme);
        }

        [Fact]
        public void AddClientCredentialsFlowSecurityDefinition_WhenSwaggerInitializerOptionsIsNull_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentNullException(
                options => options.AddClientCredentialsFlowSecurityDefinition(null)
            );
        }
        
        [Fact]
        public void AddClientCredentialsFlowSecurityDefinition_WhenTokenUrlIsNull_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentException(
                SwaggerInitializerOptionsHelpers.CreateWithNullTokenUrl(),
                (options, swaggerInitializerOptions) => options.AddClientCredentialsFlowSecurityDefinition(swaggerInitializerOptions),
                TokenUrlArgumentExceptionMessage
            );
        }

        [Fact]
        public void AddClientCredentialsFlowSecurityDefinition_WhenScopesIsNull_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentException(
                SwaggerInitializerOptionsHelpers.CreateWithNullScopes(),
                (options, swaggerInitializerOptions) => options.AddClientCredentialsFlowSecurityDefinition(swaggerInitializerOptions),
                ScopesArgumentExceptionMessage
            );
        }

        [Fact]
        public void AddClientCredentialsFlowSecurityDefinition_Success()
        {
            // Arrange
            var options = new SwaggerGenOptions();
            var initializerOptions = new SwaggerInitializerOptions
            {
                AuthorizationUrl = new Uri("http://test.dev"),
                TokenUrl = new Uri("http://test.dev"),
                Scopes = new Dictionary<string, string>
                {
                    {"TestScopeKey", "TestScopeValue"}
                }
            };

            var expectedOpenApiSecurityScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = initializerOptions.TokenUrl,
                        Scopes = initializerOptions.Scopes
                    }
                }
            };

            // Act
            options.AddClientCredentialsFlowSecurityDefinition(
                initializerOptions);

            // Assert
            Assert.Single(options.SwaggerGeneratorOptions.SecuritySchemes);

            var securityScheme = Assert.Contains(
                OAuth2Security.ClientCredentials,
                options.SwaggerGeneratorOptions.SecuritySchemes
            );

            securityScheme.Should().BeEquivalentTo(expectedOpenApiSecurityScheme);
        }

        [Fact]
        public void AddResourceOwnerFlowSecurityDefinition_WhenSwaggerInitializerOptionsIsNull_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentNullException(
                options => options.AddResourceOwnerFlowSecurityDefinition(null)
            );
        }

        [Fact]
        public void AddResourceOwnerFlowSecurityDefinition_WhenAuthorizationUrlIsNull_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentException(
                SwaggerInitializerOptionsHelpers.CreateWithNullAuthorizationUrl(),
                (options, swaggerInitializerOptions) => options.AddResourceOwnerFlowSecurityDefinition(swaggerInitializerOptions),
                AuthorizationUrlArgumentExceptionMessage
            );
        }

        [Fact]
        public void AddResourceOwnerFlowSecurityDefinition_WhenTokenUrlIsNull_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentException(
                SwaggerInitializerOptionsHelpers.CreateWithNullTokenUrl(),
                (options, swaggerInitializerOptions) => options.AddResourceOwnerFlowSecurityDefinition(swaggerInitializerOptions),
                TokenUrlArgumentExceptionMessage
            );
        }

        [Fact]
        public void AddResourceOwnerFlowSecurityDefinition_WhenScopesIsNull_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentException(
                SwaggerInitializerOptionsHelpers.CreateWithNullScopes(),
                (options, swaggerInitializerOptions) => options.AddResourceOwnerFlowSecurityDefinition(swaggerInitializerOptions),
                ScopesArgumentExceptionMessage
            );
        }

        [Fact]
        public void AddResourceOwnerFlowSecurityDefinition_Success()
        {
            // Arrange
            var options = new SwaggerGenOptions();
            var initializerOptions = new SwaggerInitializerOptions
            {
                AuthorizationUrl = new Uri("http://test.dev"),
                TokenUrl = new Uri("http://test.dev"),
                Scopes = new Dictionary<string, string>
                {
                    {"TestScopeKey", "TestScopeValue"}
                }
            };

            var expectedOpenApiSecurityScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = initializerOptions.AuthorizationUrl,
                        TokenUrl = initializerOptions.TokenUrl,
                        Scopes = initializerOptions.Scopes
                    }
                }
            };

            // Act
            options.AddResourceOwnerFlowSecurityDefinition(
                initializerOptions
            );

            // Assert
            Assert.Single(options.SwaggerGeneratorOptions.SecuritySchemes);

            var securityScheme = Assert.Contains(
                OAuth2Security.ResourceOwnerPasswordCredentials,
                options.SwaggerGeneratorOptions.SecuritySchemes
            );

            securityScheme.Should().BeEquivalentTo(expectedOpenApiSecurityScheme);
        }

        [Fact]
        public void AddImplicitFlowSecurityDefinition_WhenSwaggerInitializerOptionsIsNull_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentNullException(
                options => options.AddImplicitFlowSecurityDefinition(null)
            );
        }

        [Fact]
        public void AddImplicitFlowSecurityDefinition_WhenAuthorizationUrlIsNull_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentException(
                SwaggerInitializerOptionsHelpers.CreateWithNullAuthorizationUrl(),
                (options, swaggerInitializerOptions) => options.AddImplicitFlowSecurityDefinition(swaggerInitializerOptions),
                AuthorizationUrlArgumentExceptionMessage
            );
        }
        
        [Fact]
        public void AddImplicitFlowSecurityDefinition_WhenScopesIsNull_ThrowsArgumentException()
        {
            // Arrange & Act & Assert
            AssertExtensions.ArgumentException(
                SwaggerInitializerOptionsHelpers.CreateWithNullScopes(),
                (options, swaggerInitializerOptions) => options.AddImplicitFlowSecurityDefinition(swaggerInitializerOptions),
                ScopesArgumentExceptionMessage
            );
        }

        [Fact]
        public void AddImplicitFlowSecurityDefinition_Success()
        {
            // Arrange
            var options = new SwaggerGenOptions();
            var initializerOptions = new SwaggerInitializerOptions
            {
                AuthorizationUrl = new Uri("http://test.dev"),
                TokenUrl = new Uri("http://test.dev"),
                Scopes = new Dictionary<string, string>
                {
                    {"TestScopeKey", "TestScopeValue"}
                }
            };

            var expectedOpenApiSecurityScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = initializerOptions.AuthorizationUrl,
                        Scopes = initializerOptions.Scopes
                    }
                }
            };

            // Act
            options.AddImplicitFlowSecurityDefinition(
                initializerOptions
            );

            // Assert
            Assert.Single(options.SwaggerGeneratorOptions.SecuritySchemes);

            var securityScheme = Assert.Contains(
                OAuth2Security.Implicit,
                options.SwaggerGeneratorOptions.SecuritySchemes
            );

            securityScheme.Should().BeEquivalentTo(expectedOpenApiSecurityScheme);
        }
    }
}