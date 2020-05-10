using System;

namespace GodelTech.Microservices.Swagger.Configuration
{
    public sealed class SwaggerInitializerOptions
    {
        public ScopeDetails[] SupportedScopes { get; set; } = Array.Empty<ScopeDetails>();
        public string DocumentTitle { get; set; } = "API";
        public string DocumentVersion { get; set; } = "v1";

        public string AuthorizeEndpointUrl { get; set; }
        public string TokenEndpointUrl { get; set; }
    }
}
