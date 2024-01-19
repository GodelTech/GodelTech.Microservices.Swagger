using System;
using GodelTech.Microservices.Swagger.Utilities;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace GodelTech.Microservices.Swagger.Tests.Fakes;

public class FakeSwaggerInitializer : SwaggerInitializer
{
    public FakeSwaggerInitializer(Action<SwaggerInitializerOptions> configure, IVersion version)
        : base(configure, version)
    {

    }

    public void ExposedConfigureSwaggerGenOptions(SwaggerGenOptions options)
    {
        base.ConfigureSwaggerGenOptions(options);
    }

    public void ExposedConfigureSwaggerOptions(SwaggerOptions options)
    {
        base.ConfigureSwaggerOptions(options);
    }

    public void ExposedConfigureSwaggerUiOptions(SwaggerUIOptions options)
    {
        base.ConfigureSwaggerUiOptions(options);
    }
}
