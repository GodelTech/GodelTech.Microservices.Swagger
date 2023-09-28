using System;
using System.Collections.Generic;
using GodelTech.Microservices.Core;
using GodelTech.Microservices.Core.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GodelTech.Microservices.Swagger.Demo;

public class Startup : MicroserviceStartup
{
    public Startup(IConfiguration configuration)
        : base(configuration)
    {

    }

    protected override IEnumerable<IMicroserviceInitializer> CreateInitializers()
    {
        yield return new DeveloperExceptionPageInitializer();
        yield return new ExceptionHandlerInitializer();
        yield return new HstsInitializer();

        yield return new GenericInitializer(null, (app, _) => app.UseRouting());
        yield return new GenericInitializer(
            services =>
            {
                services
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(
                        options =>
                        {
                            options.Audience = "https://localhost";
                            options.Authority = "https://localhost/identity";
                        }
                    );
            },
            (app, _) => app.UseAuthentication()
        );
        yield return new GenericInitializer(
            services =>
            {
                services
                    .AddAuthorization(
                        options =>
                        {
                            foreach (var policy in new AuthorizationPolicyFactory().Create())
                            {
                                options.AddPolicy(policy.Key, policy.Value);
                            }
                        }
                    );
            },
            (app, _) => app.UseAuthorization()
        );

        yield return new SwaggerInitializer(
            options =>
            {
                options.DocumentTitle = "Demo API";
                options.DocumentVersion = "v2";
                options.AuthorizationUrl = new Uri("https://localhost");
                options.TokenUrl = new Uri("https://localhost/token");
                options.Scopes = new Dictionary<string, string>
                {
                    { "fake.add", "add" },
                    { "fake.edit", "edit" },
                    { "fake.delete", "delete" }
                };
            }
        );

        yield return new ApiInitializer();
    }
}
