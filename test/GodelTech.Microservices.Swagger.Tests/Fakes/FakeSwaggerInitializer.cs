using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace GodelTech.Microservices.Swagger.Tests.Fakes
{
    public class FakeSwaggerInitializer : SwaggerInitializer
    {
        public new void ConfigureSwaggerGenOptions(SwaggerGenOptions options)
        {
            base.ConfigureSwaggerGenOptions(options);
        }
        
        public new void ConfigureSwaggerUiOptions(SwaggerUIOptions options)
        {
            base.ConfigureSwaggerUiOptions(options);
        }
    }
}