using System.Collections.Generic;
using System.Linq;
using GodelTech.Microservices.Core;
using GodelTech.Microservices.Core.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GodelTech.Microservices.Swagger.Tests.Utils
{
    public class InMemoryDefaultStartup : MicroserviceStartup
    {
        private InMemoryServerSettings _settings;

        private IEnumerable<IMicroserviceInitializer> _initializers = Enumerable.Empty<IMicroserviceInitializer>();

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
            foreach (var initializer in _initializers ?? Enumerable.Empty<IMicroserviceInitializer>())
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