using System;
using System.Linq;
using GodelTech.Microservices.Swagger.Configuration;
using GodelTech.Microservices.Swagger.Swagger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GodelTech.Microservices.Swagger
{
    public static class SwaggerGenOptionsExtensions
    {
        public static void AddGenericAuthHeaderFlowSecurityDefinition(this SwaggerGenOptions options)
        {
            if (options == null) 
                throw new ArgumentNullException(nameof(options));

            options.AddSecurityDefinition(OAuth2Security.OAuth2, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\""
            });
        }

        public static void AddAuthorizationCodeSecurityDefinition(this SwaggerGenOptions options, SwaggerInitializerOptions initializerOptions)
        {
            if (options == null) 
                throw new ArgumentNullException(nameof(options));
            if (initializerOptions == null) 
                throw new ArgumentNullException(nameof(initializerOptions));
            if (string.IsNullOrWhiteSpace(initializerOptions.AuthorizeEndpointUrl))
                throw new ArgumentException("Value can't be empty or null", nameof(initializerOptions.AuthorizeEndpointUrl));
            if (string.IsNullOrWhiteSpace(initializerOptions.TokenEndpointUrl))
                throw new ArgumentException("Value can't be empty or null", nameof(initializerOptions.TokenEndpointUrl));
            if (initializerOptions.SupportedScopes == null)
                throw new ArgumentException("Value can't be empty or null", nameof(initializerOptions.SupportedScopes));

            options.AddSecurityDefinition(OAuth2Security.AuthorizationCode, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(initializerOptions.AuthorizeEndpointUrl),
                        TokenUrl = new Uri(initializerOptions.TokenEndpointUrl),
                        Scopes = initializerOptions.SupportedScopes.ToDictionary(x => x.Name, x => x.Description)
                    }
                }
            });
        }

        public static void AddClientCredentialsSecurityFlowDefinition(this SwaggerGenOptions options, SwaggerInitializerOptions initializerOptions)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (initializerOptions == null)
                throw new ArgumentNullException(nameof(initializerOptions));
            if (string.IsNullOrWhiteSpace(initializerOptions.TokenEndpointUrl))
                throw new ArgumentException("Value can't be empty or null", nameof(initializerOptions.TokenEndpointUrl));
            if (initializerOptions.SupportedScopes == null)
                throw new ArgumentException("Value can't be empty or null", nameof(initializerOptions.SupportedScopes));

            options.AddSecurityDefinition(OAuth2Security.ClientCredentials, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(initializerOptions.TokenEndpointUrl),
                        Scopes = initializerOptions.SupportedScopes.ToDictionary(x => x.Name, x => x.Description)
                    }
                }
            });
        }

        public static void AddResourceOwnerFlowSecurityDefinition(this SwaggerGenOptions options, SwaggerInitializerOptions initializerOptions)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (initializerOptions == null)
                throw new ArgumentNullException(nameof(initializerOptions));
            if (string.IsNullOrWhiteSpace(initializerOptions.AuthorizeEndpointUrl))
                throw new ArgumentException("Value can't be empty or null", nameof(initializerOptions.AuthorizeEndpointUrl));
            if (string.IsNullOrWhiteSpace(initializerOptions.TokenEndpointUrl))
                throw new ArgumentException("Value can't be empty or null", nameof(initializerOptions.TokenEndpointUrl));
            if (initializerOptions.SupportedScopes == null)
                throw new ArgumentException("Value can't be empty or null", nameof(initializerOptions.SupportedScopes));

            options.AddSecurityDefinition(OAuth2Security.ResourceOwnerPasswordCredentials,
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(initializerOptions.AuthorizeEndpointUrl),
                            TokenUrl = new Uri(initializerOptions.TokenEndpointUrl),
                            Scopes = initializerOptions.SupportedScopes.ToDictionary(x => x.Name, x => x.Description)
                        },
                    }
                });
        }

        public static void AddImplicitFlowSecurityDefinition(this SwaggerGenOptions options, SwaggerInitializerOptions initializerOptions)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (initializerOptions == null)
                throw new ArgumentNullException(nameof(initializerOptions));
            if (string.IsNullOrWhiteSpace(initializerOptions.AuthorizeEndpointUrl))
                throw new ArgumentException("Value can't be empty or null", nameof(initializerOptions.AuthorizeEndpointUrl));
            if (initializerOptions.SupportedScopes == null)
                throw new ArgumentException("Value can't be empty or null", nameof(initializerOptions.SupportedScopes));

            options.AddSecurityDefinition(OAuth2Security.Implicit, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(initializerOptions.AuthorizeEndpointUrl),
                        Scopes = initializerOptions.SupportedScopes.ToDictionary(x => x.Name, x => x.Description)
                    }
                }
            });
        }
    }
}