using Microsoft.OpenApi.Models;

namespace GodelTech.Microservices.Swagger.Filters
{
    /// <summary>
    /// OAuth2OperationFilter Helpers.
    /// </summary>
    public static class OAuth2OperationFilterHelpers
    {
        /// <summary>
        /// Creates OpenApiSecurityScheme.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>OpenApiSecurityScheme</returns>
        public static OpenApiSecurityScheme CreateOpenApiSecurityScheme(string id)
        {
            return new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = id,
                    Type = ReferenceType.SecurityScheme
                }
            };
        }
    }
}
