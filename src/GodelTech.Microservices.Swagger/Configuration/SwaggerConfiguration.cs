namespace GodelTech.Microservices.Swagger.Configuration
{
    public sealed class SwaggerConfiguration : ISwaggerConfiguration
    {
        public string AuthorizeEndpointUrl { get; set; }
        public string TokenEndpointUrl { get; set; }
        public ScopeDetails[] SupportedScopes { get; set; }
    }
}
