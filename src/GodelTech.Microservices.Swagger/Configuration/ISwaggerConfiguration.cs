namespace GodelTech.Microservices.Swagger.Configuration
{
    public interface ISwaggerConfiguration
    {
        string AuthorizeEndpointUrl { get; }
        string TokenEndpointUrl { get; }
        ScopeDetails[] SupportedScopes { get; }
    }
}