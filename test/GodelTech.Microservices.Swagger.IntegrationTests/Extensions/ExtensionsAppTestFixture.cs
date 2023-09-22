using System;
using System.IO;
using GodelTech.Microservices.Swagger.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;

namespace GodelTech.Microservices.Swagger.IntegrationTests.Extensions;

public class ExtensionsAppTestFixture : WebApplicationFactory<TestStartup>
{
    protected override IHostBuilder CreateHostBuilder()
    {
        var builder = Host
            .CreateDefaultBuilder()
            .ConfigureWebHostDefaults(
                x =>
                {
                    x.UseStartup<TestStartup>()
                        .UseTestServer();
                }
            );

        return builder;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        builder.UseSetting("https_port", "8080");

        builder
            .Configure(
                app =>
                {
                    app.UseRouting();

                    app.AddSwaggerRedirectHomePage();
                }
            );
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseContentRoot(Directory.GetCurrentDirectory());

        return base.CreateHost(builder);
    }
}
