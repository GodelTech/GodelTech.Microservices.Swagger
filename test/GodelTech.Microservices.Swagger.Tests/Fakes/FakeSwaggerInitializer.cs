using System;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace GodelTech.Microservices.Swagger.Tests.Fakes
{
    public class FakeSwaggerInitializer : SwaggerInitializer
    {
        public FakeSwaggerInitializer(Action<SwaggerInitializerOptions> configure)
            : base(configure)
        {

        }

        public void ExposedConfigureSwaggerGenOptions(SwaggerGenOptions options)
        {
            base.ConfigureSwaggerGenOptions(options);
        }
        
        public void ExposedConfigureSwaggerUiOptions(SwaggerUIOptions options)
        {
            base.ConfigureSwaggerUiOptions(options);
        }
    }
}