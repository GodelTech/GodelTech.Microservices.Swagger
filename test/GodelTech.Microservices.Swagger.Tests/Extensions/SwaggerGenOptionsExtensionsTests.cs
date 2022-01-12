using System;
using System.Collections.Generic;
using GodelTech.Microservices.Swagger.Extensions;
using GodelTech.Microservices.Swagger.Swagger;
using GodelTech.Microservices.Swagger.Tests.Fakes;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace GodelTech.Microservices.Swagger.Tests.Extensions
{
    public class SwaggerGenOptionsExtensionsTests
    {
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
                Description =
                    "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\""
            };

            // Act
            options.AddAuthHeaderFlowSecurityDefinition();

            // Assert
            var securityScheme = Assert.Contains(
                OAuth2Security.OAuth2,
                options.SwaggerGeneratorOptions.SecuritySchemes
            );

            Assert.Equal(
                expectedOpenApiSecurityScheme,
                securityScheme,
                new OpenApiSecuritySchemeComparer()
            );
        }

        #region AddAuthorizationCodeFlowSecurityDefinitionTests

        [Fact]
        public void
            AddAuthorizationCodeFlowSecurityDefinition_ThrowsArgumentNullException_When_SwaggerInitializerOptionsIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => options
                    .AddAuthorizationCodeFlowSecurityDefinition(null)
            );
            Assert.Equal("initializerOptions", exception.ParamName);
        }

        [Fact]
        public void
            AddAuthorizationCodeFlowSecurityDefinition_ThrowsArgumentException_When_AuthorizationUrlIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentException>(
                () => options
                    .AddAuthorizationCodeFlowSecurityDefinition(
                        new SwaggerInitializerOptions
                        {
                            AuthorizationUrl = null
                        }
                    )
            );
            Assert.Equal("AuthorizationUrl can't be null", exception.Message);
        }


        [Fact]
        public void
            AddAuthorizationCodeFlowSecurityDefinition_ThrowsArgumentException_When_TokenUrlIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentException>(
                () => options
                    .AddAuthorizationCodeFlowSecurityDefinition(
                        new SwaggerInitializerOptions
                        {
                            AuthorizationUrl = new Uri("http://test.dev"),
                            TokenUrl = null
                        }
                    )
            );
            Assert.Equal("TokenUrl can't be null", exception.Message);
        }

        [Fact]
        public void
            AddAuthorizationCodeFlowSecurityDefinition_ThrowsArgumentException_When_ScopesIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentException>(
                () => options
                    .AddAuthorizationCodeFlowSecurityDefinition(
                        new SwaggerInitializerOptions
                        {
                            AuthorizationUrl = new Uri("http://test.dev"),
                            TokenUrl = new Uri("http://test.dev"),
                            Scopes = null
                        }
                    )
            );
            Assert.Equal("Scopes can't be null", exception.Message);
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
            var securityScheme = Assert.Contains(
                OAuth2Security.AuthorizationCode,
                options.SwaggerGeneratorOptions.SecuritySchemes
            );

            Assert.Equal(expectedOpenApiSecurityScheme, securityScheme, new OpenApiSecuritySchemeComparer());
        }

        #endregion

        #region AddClientCredentialsFlowSecurityDefinition

        [Fact]
        public void
            AddClientCredentialsFlowSecurityDefinition_ThrowsArgumentNullException_When_SwaggerInitializerOptionsIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => options
                    .AddClientCredentialsFlowSecurityDefinition(null)
            );
            Assert.Equal("initializerOptions", exception.ParamName);
        }
        
        [Fact]
        public void
            AddClientCredentialsFlowSecurityDefinition_ThrowsArgumentException_When_TokenUrlIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentException>(
                () => options
                    .AddClientCredentialsFlowSecurityDefinition(
                        new SwaggerInitializerOptions
                        {
                            TokenUrl = null
                        }
                    )
            );
            Assert.Equal("TokenUrl can't be null", exception.Message);
        }

        [Fact]
        public void
            AddClientCredentialsFlowSecurityDefinition_ThrowsArgumentException_When_ScopesIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentException>(
                () => options
                    .AddClientCredentialsFlowSecurityDefinition(
                        new SwaggerInitializerOptions
                        {
                            TokenUrl = new Uri("http://test.dev"),
                            Scopes = null
                        }
                    )
            );
            Assert.Equal("Scopes can't be null", exception.Message);
        }



        #endregion

        #region AddResourceOwnerFlowSecurityDefinitionTests

        [Fact]
        public void
            AddResourceOwnerFlowSecurityDefinition_ThrowsArgumentNullException_When_SwaggerInitializerOptionsIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => options
                    .AddResourceOwnerFlowSecurityDefinition(null)
            );
            Assert.Equal("initializerOptions", exception.ParamName);
        }

        [Fact]
        public void
            AddResourceOwnerFlowSecurityDefinition_ThrowsArgumentException_When_AuthorizationUrlIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentException>(
                () => options
                    .AddResourceOwnerFlowSecurityDefinition(
                        new SwaggerInitializerOptions
                        {
                            AuthorizationUrl = null
                        }
                    )
            );
            Assert.Equal("AuthorizationUrl can't be null", exception.Message);
        }

        [Fact]
        public void
            AddResourceOwnerFlowSecurityDefinition_ThrowsArgumentException_When_TokenUrlIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentException>(
                () => options
                    .AddResourceOwnerFlowSecurityDefinition(
                        new SwaggerInitializerOptions
                        {
                            AuthorizationUrl = new Uri("http://test.dev"),
                            TokenUrl = null
                        }
                    )
            );
            Assert.Equal("TokenUrl can't be null", exception.Message);
        }

        [Fact]
        public void
            AddResourceOwnerFlowSecurityDefinition_ThrowsArgumentException_When_ScopesIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentException>(
                () => options
                    .AddResourceOwnerFlowSecurityDefinition(
                        new SwaggerInitializerOptions
                        {
                            AuthorizationUrl = new Uri("http://test.dev"),
                            TokenUrl = new Uri("http://test.dev"),
                            Scopes = null
                        }
                    )
            );
            Assert.Equal("Scopes can't be null", exception.Message);
        }

        #endregion

        #region AddImplicitFlowSecurityDefinition

        [Fact]
        public void
            AddImplicitFlowSecurityDefinition_ThrowsArgumentNullException_When_SwaggerInitializerOptionsIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => options
                    .AddImplicitFlowSecurityDefinition(null)
            );
            Assert.Equal("initializerOptions", exception.ParamName);
        }

        [Fact]
        public void
            AddImplicitFlowSecurityDefinition_ThrowsArgumentException_When_AuthorizationUrlIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentException>(
                () => options
                    .AddImplicitFlowSecurityDefinition(
                        new SwaggerInitializerOptions
                        {
                            AuthorizationUrl = null
                        }
                    )
            );
            Assert.Equal("AuthorizationUrl can't be null", exception.Message);
        }
        
        [Fact]
        public void
            AddImplicitFlowSecurityDefinition_ThrowsArgumentException_When_ScopesIsNull()
        {
            // Arrange 
            var options = new SwaggerGenOptions();

            // Act && Assert
            var exception = Assert.Throws<ArgumentException>(
                () => options
                    .AddImplicitFlowSecurityDefinition(
                        new SwaggerInitializerOptions
                        {
                            AuthorizationUrl = new Uri("http://test.dev"),
                            Scopes = null
                        }
                    )
            );
            Assert.Equal("Scopes can't be null", exception.Message);
        }

        #endregion
    }
}