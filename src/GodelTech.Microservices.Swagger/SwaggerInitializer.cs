using System;
using System.Collections.Generic;
using System.Linq;
using GodelTech.Microservices.Core;
using GodelTech.Microservices.Core.Mvc;
using GodelTech.Microservices.Swagger.Configuration;
using GodelTech.Microservices.Swagger.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GodelTech.Microservices.Swagger
{
    public class SwaggerInitializer : MicroserviceInitializerBase
    {
        public SwaggerInitializerOptions Options { get; set; } = new SwaggerInitializerOptions();

        public SwaggerInitializer(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Details can be found here https://github.com/domaindrivendev/Swashbuckle.AspNetCore
            // Default address http://localhost:5000/swagger/
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{Options.DocumentVersion}/swagger.json", "v1");
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            var scopes = Options.SupportedScopes.ToDictionary(x => x.Name, x => x.Description);

            services.AddSwaggerGen(options =>
            {
                AddGenericAuthHeaderFlowSecurityDefinition(options);

                if (!string.IsNullOrWhiteSpace(Options.AuthorizeEndpointUrl) &&
                    !string.IsNullOrWhiteSpace(Options.TokenEndpointUrl))
                    AddAuthorizationCodeSecurityDefinition(options, scopes);

                if (!string.IsNullOrWhiteSpace(Options.AuthorizeEndpointUrl))
                    AddImplicitFlowSecurityDefinition(options, scopes);


                if (!string.IsNullOrWhiteSpace(Options.AuthorizeEndpointUrl) &&
                    !string.IsNullOrWhiteSpace(Options.TokenEndpointUrl))
                    AddResourceOwnerFlowSecurityDefinition(options, scopes);

                if (!string.IsNullOrWhiteSpace(Options.TokenEndpointUrl))
                    AddClientCredentialsSecurityFlowDefinition(options, scopes);

                options.SwaggerDoc(Options.DocumentVersion, new OpenApiInfo
                {
                    Title = Options.DocumentTitle, 
                    Version = Options.DocumentVersion
                });

                options.EnableAnnotations();
                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }

        private static void AddGenericAuthHeaderFlowSecurityDefinition(SwaggerGenOptions options)
        {
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

        private void AddClientCredentialsSecurityFlowDefinition(SwaggerGenOptions options, IDictionary<string, string> scopes)
        {
            options.AddSecurityDefinition(OAuth2Security.ClientCredentials, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    ClientCredentials = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri(Options.TokenEndpointUrl),
                        Scopes = scopes
                    }
                }
            });
        }

        private void AddResourceOwnerFlowSecurityDefinition(SwaggerGenOptions options, IDictionary<string, string> scopes)
        {
            options.AddSecurityDefinition(OAuth2Security.ResourceOwnerPasswordCredentials,
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(Options.AuthorizeEndpointUrl),
                            TokenUrl = new Uri(Options.TokenEndpointUrl),
                            Scopes = scopes
                        },
                    }
                });
        }

        private void AddImplicitFlowSecurityDefinition(SwaggerGenOptions options, IDictionary<string, string> scopes)
        {
            options.AddSecurityDefinition(OAuth2Security.Implicit, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(Options.AuthorizeEndpointUrl),
                        Scopes = scopes
                    }
                }
            });
        }

        private void AddAuthorizationCodeSecurityDefinition(SwaggerGenOptions options, IDictionary<string, string> scopes)
        {
            options.AddSecurityDefinition(OAuth2Security.AuthorizationCode, new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(Options.AuthorizeEndpointUrl),
                        TokenUrl = new Uri(Options.TokenEndpointUrl),
                        Scopes = scopes
                    }
                }
            });
        }
    }
}
