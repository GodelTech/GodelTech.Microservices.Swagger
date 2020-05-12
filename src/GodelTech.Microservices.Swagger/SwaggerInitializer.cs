using System;
using System.Linq;
using GodelTech.Microservices.Core;
using GodelTech.Microservices.Swagger.Configuration;
using GodelTech.Microservices.Swagger.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace GodelTech.Microservices.Swagger
{
    public class SwaggerInitializer : MicroserviceInitializerBase
    {
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
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            var config = GetSwaggerConfiguration();
            var scopes = config.SupportedScopes.ToDictionary(x => x.Name, x => x.Description);

            services.AddSwaggerGen(options =>
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

                options.AddSecurityDefinition(OAuth2Security.AuthorizationCode, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(config.AuthorizeEndpointUrl),
                            TokenUrl = new Uri(config.TokenEndpointUrl),
                            Scopes = scopes
                        }
                    }
                });

                options.AddSecurityDefinition(OAuth2Security.Implicit, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(config.AuthorizeEndpointUrl),
                            Scopes = scopes
                        }
                    }
                });

                options.AddSecurityDefinition(OAuth2Security.ResourceOwnerPasswordCredentials, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(config.AuthorizeEndpointUrl),
                            TokenUrl = new Uri(config.TokenEndpointUrl),
                            Scopes = scopes
                        },
                    }
                });

                options.AddSecurityDefinition(OAuth2Security.ClientCredentials, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri(config.TokenEndpointUrl),
                            Scopes = scopes
                        }
                    }
                });

                options.SwaggerDoc("v1", new OpenApiInfo { Title = "ReviewItEasy API", Version = "v1" });

                options.EnableAnnotations();

                options.OperationFilter<AuthorizeCheckOperationFilter>();
            });
        }

        public ISwaggerConfiguration GetSwaggerConfiguration()
        {
            var identityConfiguration = new SwaggerConfiguration();

            Configuration.Bind("SwaggerConfiguration", identityConfiguration);

            return identityConfiguration;
        }
    }
}
