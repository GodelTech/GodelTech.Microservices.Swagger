using System;
using System.Collections.Generic;
using System.Net.Http;
using GodelTech.Microservices.Core;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;

namespace GodelTech.Microservices.Swagger.Tests.Utils
{
    [TestFixture]
    public abstract class InMemoryServerTest
    {
        protected HttpClient Client;
        private TestServer _server;

        protected void UseInMemoryServer(
            Func<IConfiguration, IEnumerable<IMicroserviceInitializer>> initializerProvider,
            Type[] controllerTypes,
            Action<MvcOptions> configFunc = null,
            Action<IConfiguration, IServiceCollection> configureServices = null)
        {
            var host = new HostBuilder()
                .ConfigureWebHost(webBuilder => {
                    webBuilder
                        .UseTestServer()
                        .ConfigureTestServices(x =>
                            RegisterSettings(x, controllerTypes, configFunc, configureServices, initializerProvider))
                        .UseStartup<InMemoryDefaultStartup>();
                }).Start();

            _server = host.GetTestServer();

            var tmpClient = host.GetTestClient();
            var responseVersionHandler = new ResponseVersionHandler { InnerHandler = _server.CreateHandler() };

            Client = new HttpClient(responseVersionHandler)
            {
                BaseAddress = tmpClient.BaseAddress,
                DefaultRequestVersion = new Version(2, 0)
            };
        }

        private static void RegisterSettings(
            IServiceCollection services,
            Type[] controllerTypes,
            Action<MvcOptions> configFunc,
            Action<IConfiguration, IServiceCollection> configureServices,
            Func<IConfiguration, IEnumerable<IMicroserviceInitializer>> initializers)
        {
            var settings = new InMemoryServerSettings
            {
                ControllerTypes = controllerTypes,
                MvcOptionsConfig = configFunc,
                ConfigureServices = configureServices,
                Initializers = initializers
            };

            services.AddSingleton(settings);
        }

        protected T ResolveServiceFromContainer<T>()
        {
            return (T)_server.Services.GetService(typeof(T));
        }

        protected IDataProtector GetProtector(string purpose)
        {
            return _server.Services.GetDataProtector(purpose);
        }

        [TearDown]
        public void Teardown()
        {
            _server?.Dispose();
            _server = null;
        }
    }
}
