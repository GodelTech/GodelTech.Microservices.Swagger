using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GodelTech.Microservices.Swagger.Filters
{
    /// <summary>
    /// OAuth2 OperationFilter.
    /// </summary>
    public class OAuth2OperationFilter : IOperationFilter
    {
        /// <summary>
        /// Apply filter.
        /// </summary>
        /// <param name="operation">OpenApiOperation.</param>
        /// <param name="context">OperationFilterContext.</param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.MethodInfo.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute)) return;

            var authorizeAttributes = new List<AuthorizeAttribute>();
            var swaggerSecurityAttributes = new List<SwaggerSecurityAttribute>();

            // Get method attributes
            authorizeAttributes.AddRange(context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>());
            swaggerSecurityAttributes.AddRange(context.MethodInfo.GetCustomAttributes(true).OfType<SwaggerSecurityAttribute>());

            // Get class attributes
            if (context.MethodInfo.DeclaringType != null)
            {
                authorizeAttributes.AddRange(context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>());
                swaggerSecurityAttributes.AddRange(context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<SwaggerSecurityAttribute>());
            }

            var hasAuthorize = authorizeAttributes.Any() || swaggerSecurityAttributes.Any();

            if (!hasAuthorize) return;

            var scopes = new List<string>();

            foreach (var swaggerSecurityAttribute in swaggerSecurityAttributes)
            {
                scopes = scopes.Union(swaggerSecurityAttribute.Scopes).ToList();
            }

            // Initialize the operation.Security property
            if (operation.Security == null)
            {
                operation.Security = new List<OpenApiSecurityRequirement>();
            }

            // Add response types on secure APIs
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

            operation.Security.Add(
                new OpenApiSecurityRequirement
                {
                    {
                        OAuth2OperationFilterHelpers.CreateOpenApiSecurityScheme(OAuth2Security.OAuth2),
                        scopes
                    },
                    {
                        OAuth2OperationFilterHelpers.CreateOpenApiSecurityScheme(OAuth2Security.AuthorizationCode),
                        scopes
                    },
                    {
                        OAuth2OperationFilterHelpers.CreateOpenApiSecurityScheme(OAuth2Security.ClientCredentials),
                        scopes
                    },
                    {
                        OAuth2OperationFilterHelpers.CreateOpenApiSecurityScheme(OAuth2Security.ResourceOwnerPasswordCredentials),
                        scopes
                    },
                    {
                        OAuth2OperationFilterHelpers.CreateOpenApiSecurityScheme(OAuth2Security.Implicit),
                        scopes
                    }
                }
            );
        }
    }
}
