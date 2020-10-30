using System;
using GodelTech.Microservices.Core;
using GodelTech.Microservices.Swagger.Configuration;
using GodelTech.Microservices.Swagger.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace GodelTech.Microservices.Swagger
{
    public class SwaggerInitializer : MicroserviceInitializerBase
    {
        private SwaggerInitializerOptions _options = new SwaggerInitializerOptions();

        public SwaggerInitializerOptions Options
        {
            get => _options;
            set => _options = value ?? throw new ArgumentNullException(nameof(value));
        }

        public SwaggerInitializer(IConfiguration configuration)
            : base(configuration)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(ConfigureSwaggerGenOptions);
        }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (app == null) 
                throw new ArgumentNullException(nameof(app));
            if (env == null) 
                throw new ArgumentNullException(nameof(env));

            // Details can be found here https://github.com/domaindrivendev/Swashbuckle.AspNetCore
            // Default address http://localhost:5000/swagger/
            app.UseSwagger(ConfigureSwaggerOptions);
            app.UseSwaggerUI(ConfigureSwaggerUiOptions);
        }

        protected virtual void ConfigureSwaggerOptions(SwaggerOptions options)
        {
            options.RouteTemplate = "swagger/{documentName}/swagger.json";
        }

        protected virtual void ConfigureSwaggerUiOptions(SwaggerUIOptions options)
        {
            options.SwaggerEndpoint($"/swagger/{Options.DocumentVersion}/swagger.json", "v1");
        }

        protected virtual void ConfigureSwaggerGenOptions(SwaggerGenOptions options)
        {
            options.AddGenericAuthHeaderFlowSecurityDefinition();

            if (!string.IsNullOrWhiteSpace(Options.AuthorizeEndpointUrl) &&
                !string.IsNullOrWhiteSpace(Options.TokenEndpointUrl))
                options.AddAuthorizationCodeSecurityDefinition(Options);

            if (!string.IsNullOrWhiteSpace(Options.AuthorizeEndpointUrl))
                options.AddImplicitFlowSecurityDefinition(Options);

            if (!string.IsNullOrWhiteSpace(Options.AuthorizeEndpointUrl) &&
                !string.IsNullOrWhiteSpace(Options.TokenEndpointUrl))
                options.AddResourceOwnerFlowSecurityDefinition(Options);

            if (!string.IsNullOrWhiteSpace(Options.TokenEndpointUrl))
                options.AddClientCredentialsSecurityFlowDefinition(Options);

            options.SwaggerDoc(
                Options.DocumentVersion,
                new OpenApiInfo
                {
                    Title = Options.DocumentTitle,
                    Version = Options.DocumentVersion
                }
            );

            options.EnableAnnotations();
            options.OperationFilter<AuthorizeCheckOperationFilter>();

            if (!string.IsNullOrWhiteSpace(Options.XmlCommentsFilePath))
                options.IncludeXmlComments(Options.XmlCommentsFilePath, true);
        }
    }
}
