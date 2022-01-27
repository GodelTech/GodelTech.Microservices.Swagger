using GodelTech.Microservices.Swagger.Demo;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace GodelTech.Microservices.Swagger.IntegrationTests
{
    public class AppTestFixture : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSetting("https_port", "8080");
        }
    }
}