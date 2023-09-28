using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace GodelTech.Microservices.Swagger.Extensions
{
    /// <summary>
    /// Application builder extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds redirect from home page to swagger UI.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="swaggerUiOptions"><see cref="SwaggerUIOptions"/>.</param>
        public static void AddSwaggerRedirectHomePage(this IApplicationBuilder app, SwaggerUIOptions swaggerUiOptions = default)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (swaggerUiOptions == null) swaggerUiOptions = new SwaggerUIOptions();

            app.UseEndpoints(
                builder => builder.MapGet(
                    string.Empty,
                    context =>
                    {
                        context.Response.Redirect(swaggerUiOptions.RoutePrefix, true);
                        return Task.CompletedTask;
                    }
                )
            );
        }
    }
}
