using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GodelTech.Microservices.Swagger.Extensions
{
    /// <summary>
    /// Extensions of filter SwaggerGen options.
    /// </summary>
    public static class SwaggerGenOptionsExtensions
    {
        /// <summary>
        /// Add AuthHeaderFlow "securityDefinitions", describing how your API is protected, to the generated Swagger
        /// </summary>
        /// <param name="options">The SwaggerGen options.</param>
        public static void AddAuthHeaderFlowSecurityDefinition(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition(
                OAuth2Security.OAuth2,
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\""
                }
            );
        }

        /// <summary>
        /// Add AuthorizationCodeFlow "securityDefinitions", describing how your API is protected, to the generated Swagger
        /// </summary>
        /// <param name="options">The SwaggerGen options.</param>
        /// <param name="initializerOptions">The Swagger initializer options.</param>
        public static void AddAuthorizationCodeFlowSecurityDefinition(
            this SwaggerGenOptions options,
            SwaggerInitializerOptions initializerOptions)
        {
            ArgumentNullException.ThrowIfNull(initializerOptions);
            if (initializerOptions.AuthorizationUrl == null) throw new ArgumentException("AuthorizationUrl can't be null");
            if (initializerOptions.TokenUrl == null) throw new ArgumentException("TokenUrl can't be null");
            if (initializerOptions.Scopes == null) throw new ArgumentException("Scopes can't be null");

            options.AddSecurityDefinition(
                OAuth2Security.AuthorizationCode,
                new OpenApiSecurityScheme
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
                }
            );
        }

        /// <summary>
        /// Add ClientCredentialsFlow "securityDefinitions", describing how your API is protected, to the generated Swagger
        /// </summary>
        /// <param name="options">The SwaggerGen options.</param>
        /// <param name="initializerOptions">The Swagger initializer options.</param>
        public static void AddClientCredentialsFlowSecurityDefinition(
            this SwaggerGenOptions options,
            SwaggerInitializerOptions initializerOptions)
        {
            ArgumentNullException.ThrowIfNull(initializerOptions);
            if (initializerOptions.TokenUrl == null) throw new ArgumentException("TokenUrl can't be null");
            if (initializerOptions.Scopes == null) throw new ArgumentException("Scopes can't be null");

            options.AddSecurityDefinition(
                OAuth2Security.ClientCredentials,
                new OpenApiSecurityScheme
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
                }
            );
        }

        /// <summary>
        /// Add ResourceOwnerFlow "securityDefinitions", describing how your API is protected, to the generated Swagger
        /// </summary>
        /// <param name="options">The SwaggerGen options.</param>
        /// <param name="initializerOptions">The Swagger initializer options.</param>
        public static void AddResourceOwnerFlowSecurityDefinition(
            this SwaggerGenOptions options,
            SwaggerInitializerOptions initializerOptions)
        {
            ArgumentNullException.ThrowIfNull(initializerOptions);
            if (initializerOptions.AuthorizationUrl == null) throw new ArgumentException("AuthorizationUrl can't be null");
            if (initializerOptions.TokenUrl == null) throw new ArgumentException("TokenUrl can't be null");
            if (initializerOptions.Scopes == null) throw new ArgumentException("Scopes can't be null");

            options.AddSecurityDefinition(
                OAuth2Security.ResourceOwnerPasswordCredentials,
                new OpenApiSecurityScheme
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
                }
            );
        }

        /// <summary>
        /// Add ImplicitFlow "securityDefinitions", describing how your API is protected, to the generated Swagger
        /// </summary>
        /// <param name="options">The SwaggerGen options.</param>
        /// <param name="initializerOptions">The Swagger initializer options.</param>
        public static void AddImplicitFlowSecurityDefinition(this SwaggerGenOptions options, SwaggerInitializerOptions initializerOptions)
        {
            ArgumentNullException.ThrowIfNull(initializerOptions);
            if (initializerOptions.AuthorizationUrl == null) throw new ArgumentException("AuthorizationUrl can't be null");
            if (initializerOptions.Scopes == null) throw new ArgumentException("Scopes can't be null");

            options.AddSecurityDefinition(
                OAuth2Security.Implicit,
                new OpenApiSecurityScheme
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
                }
            );
        }
    }
}
