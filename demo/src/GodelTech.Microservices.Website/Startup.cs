using GodelTech.Microservices.Core;
using GodelTech.Microservices.Core.Mvc;
using GodelTech.Microservices.Security;
using GodelTech.Microservices.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace GodelTech.Microservices.Website
{
    public class Startup : MicroserviceStartup
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {

        }

        protected override IEnumerable<IMicroserviceInitializer> CreateInitializers()
        {
            yield return new DeveloperExceptionPageInitializer(Configuration);
            yield return new HttpsInitializer(Configuration);

            yield return new GenericInitializer((app, env) => app.UseStaticFiles());
            yield return new GenericInitializer((app, env) => app.UseRouting());
            yield return new GenericInitializer((app, env) => app.UseAuthentication());

            yield return new SwaggerInitializer(Configuration)
            {
                Options = Configuration.GetSection("SwaggerInitializerOptions").Get<SwaggerInitializerOptions>()
            };

            yield return new ApiSecurityInitializer(Configuration, new PolicyFactory());
            yield return new ApiInitializer(Configuration);
        }
    }
}
