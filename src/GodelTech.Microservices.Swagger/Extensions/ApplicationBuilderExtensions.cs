using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        /// <param name="swaggerUiOptionsConfigure">An <see cref="Action{SwaggerUIOptions}"/> to configure the provided <see cref="SwaggerUIOptions"/>.</param>
        public static void AddSwaggerRedirectHomePage(this IApplicationBuilder app, Action<SwaggerUIOptions> swaggerUiOptionsConfigure = null)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            SwaggerUIOptions swaggerUiOptions;
            using (var scope = app.ApplicationServices.CreateScope())
            {
                swaggerUiOptions = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<SwaggerUIOptions>>().Value;
                swaggerUiOptionsConfigure?.Invoke(swaggerUiOptions);
            }

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