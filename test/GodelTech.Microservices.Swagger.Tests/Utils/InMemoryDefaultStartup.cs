using System;
using System.Collections.Generic;
using System.Linq;
using GodelTech.Microservices.Core;
using GodelTech.Microservices.Core.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GodelTech.Microservices.Swagger.Tests.Utils
{
    public class InMemoryDefaultStartup : MicroserviceStartup
    {
        private InMemoryServerSettings _settings;

        private Func<IConfiguration, IEnumerable<IMicroserviceInitializer>> _initializers = x => Enumerable.Empty<IMicroserviceInitializer>();

        public InMemoryDefaultStartup()
            : base(CreateConfiguration())
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            _settings = services.BuildServiceProvider().GetService<InMemoryServerSettings>();

            _initializers = _settings.Initializers;

            base.ConfigureServices(services);
        }

        protected override IEnumerable<IMicroserviceInitializer> CreateInitializers()
        {
            yield return new GenericInitializer((app, env) => app.UseRouting());

            foreach (var initializer in _initializers(Configuration))
            {
                yield return initializer;
            }

            yield return new ApiInitializer(Configuration);
        }

        private static IConfigurationRoot CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["setting:setting"] = "value",
                })
                .Build();
        }
    }
}