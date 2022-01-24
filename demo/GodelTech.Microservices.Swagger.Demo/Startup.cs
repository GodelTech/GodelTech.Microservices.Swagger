using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using GodelTech.Microservices.Core;
using GodelTech.Microservices.Core.Mvc;

namespace GodelTech.Microservices.Swagger.Demo
{
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

            yield return new GenericInitializer(null, (app, env) => app.UseRouting());
            yield return new GenericInitializer(null, (app, env) => app.UseAuthentication());

            yield return new SwaggerInitializer(
                options =>
                {
                    options.DocumentTitle = "Demo API";
                    options.DocumentVersion = "v1";
                    options.AuthorizationUrl = new Uri("http://authorize.url");
                    options.TokenUrl = new Uri("http://token.url");
                    options.Scopes = new Dictionary<string, string>
                    {
                        { "Scope1", "Scope description" }
                    };
                }
            );

            yield return new ApiInitializer();
        }
    }
}
