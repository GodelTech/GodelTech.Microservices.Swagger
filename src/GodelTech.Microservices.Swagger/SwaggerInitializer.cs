using System;
using GodelTech.Microservices.Core;
using GodelTech.Microservices.Swagger.Configuration;
using GodelTech.Microservices.Swagger.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace GodelTech.Microservices.Swagger
{
    /// <summary>
    /// Swagger initializer.
    /// </summary>
    public class SwaggerInitializer : IMicroserviceInitializer
    {
        // todo: think about use directly SwaggerOptions, SwaggerUIOptions, SwaggerGenOptions and OpenApiInfo
        private readonly SwaggerInitializerOptions _options = new SwaggerInitializerOptions();

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerInitializer"/> class.
        /// </summary>
        /// <param name="configure">An <see cref="Action{SwaggerInitializerOptions}"/> to configure the provided <see cref="SwaggerInitializerOptions"/>.</param>
        public SwaggerInitializer(Action<SwaggerInitializerOptions> configure = null)
        {
            configure?.Invoke(_options);
        }

        /// <inheritdoc />
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(ConfigureSwaggerGenOptions);
        }

        /// <inheritdoc />
        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger(ConfigureSwaggerOptions);
            app.UseSwaggerUI(ConfigureSwaggerUiOptions);
        }

        /// <summary>
        /// Configure SwaggerGenOptions.
        /// </summary>
        /// <param name="options">SwaggerGenOptions.</param>
        protected virtual void ConfigureSwaggerGenOptions(SwaggerGenOptions options)
        {
            // todo: check what options need to be changed
            options.AddGenericAuthHeaderFlowSecurityDefinition();

            if (!string.IsNullOrWhiteSpace(_options.AuthorizeEndpointUrl) &&
                !string.IsNullOrWhiteSpace(_options.TokenEndpointUrl))
                options.AddAuthorizationCodeSecurityDefinition(_options);

            if (!string.IsNullOrWhiteSpace(_options.AuthorizeEndpointUrl))
                options.AddImplicitFlowSecurityDefinition(_options);

            if (!string.IsNullOrWhiteSpace(_options.AuthorizeEndpointUrl) &&
                !string.IsNullOrWhiteSpace(_options.TokenEndpointUrl))
                options.AddResourceOwnerFlowSecurityDefinition(_options);

            if (!string.IsNullOrWhiteSpace(_options.TokenEndpointUrl))
                options.AddClientCredentialsSecurityFlowDefinition(_options);

            options.SwaggerDoc(_options.DocumentVersion, new OpenApiInfo
            {
                Title = _options.DocumentTitle,
                Version = _options.DocumentVersion
            });

            options.EnableAnnotations();
            options.OperationFilter<AuthorizeCheckOperationFilter>();

            if (!string.IsNullOrWhiteSpace(_options.XmlCommentsFilePath))
                options.IncludeXmlComments(_options.XmlCommentsFilePath, true);
        }

        /// <summary>
        /// Configure SwaggerOptions.
        /// </summary>
        /// <param name="options">SwaggerOptions.</param>
        protected virtual void ConfigureSwaggerOptions(SwaggerOptions options)
        {

        }

        /// <summary>
        /// Configure SwaggerUIOptions.
        /// </summary>
        /// <param name="options">SwaggerUIOptions.</param>
        protected virtual void ConfigureSwaggerUiOptions(SwaggerUIOptions options)
        {
            // todo: check what options need to be changed
            options.SwaggerEndpoint($"/swagger/{_options.DocumentVersion}/swagger.json", "v1");
        }
    }
}
