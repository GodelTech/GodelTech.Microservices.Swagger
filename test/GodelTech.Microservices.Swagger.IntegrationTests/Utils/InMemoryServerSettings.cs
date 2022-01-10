//using System;
//using System.Collections.Generic;
//using System.Linq;
//using GodelTech.Microservices.Core;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;

//namespace GodelTech.Microservices.Swagger.Tests.Utils
//{
//    public class InMemoryServerSettings
//    {
//        public Type[] ControllerTypes { get; set; } = { };
//        public Action<MvcOptions> MvcOptionsConfig { get; set; }
//        public Action<IConfiguration, IServiceCollection> ConfigureServices { get; set; }
//        public Func<IConfiguration, IEnumerable<IMicroserviceInitializer>> Initializers { get; set; } = x => Enumerable.Empty<MicroserviceInitializerBase>();
//    }
//}