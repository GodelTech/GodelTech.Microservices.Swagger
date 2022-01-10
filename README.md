# GodelTech.Microservices.Swagger 
## Overview

**GodelTech.Microservices.Swagger** project provides initializer which configures Swagger endpoinds and Swagger UI. Default configuration looks as follows:

* Swagger UI can be found at [http://yourwebsite.com/swagger/index.html](http://yourwebsite.com/swagger/index.html)
* Swagger document can be found at [http://yourwebsite/swagger/v1/swagger.json](http://yourwebsite/swagger/v1/swagger.json)
  
Default behavior can be overriden by changing values of intializer's `Options` property or by deriving your customer initializer from `SwaggerInitializer`.

## Quick Start

Simplest usage of swagger initializer may look as follows:

```c#
    public sealed class Startup : MicroserviceStartup
    {
        public Startup(IConfiguration configuration)
            : base(configuration)
        {

        }

        protected override IEnumerable<IMicroserviceInitializer> CreateInitializers()
        {
            ...
            yield return new SwaggerInitializer(Configuration)
            {
                Options =
                {
                    AuthorizeEndpointUrl = "http://authorize.url",
                    TokenEndpointUrl = "http://token.url",
                    DocumentTitle = "CRM API",
                    SupportedScopes = new[]
                    {
                        new ScopeDetails { Name = "Scope1", Description = "Scope description" }
                    }
                }
            };
            ...
        }
    }
```
This code snippet adds swagger endpoints to your application and exposes document for once version of your API.

## Configuration Options

Easiest way to configure initializer is to use properties of `SwaggerInitializerOptions` class. THe following table contains list of available settings:

| Property | Description |
|---|---|
| `DocumentTitle` | Title of your Swagger document. Default value is `API`. |
| `DocumentVersion` | Version of API exposed by service. Default value is `v1` |
| `AuthorizeEndpointUrl` | *(Optional)* Identity provider endpoint used by Swagger UI to authorize user. If this value is not defined some OAuth flows might not be available in Swagger UI |
| `TokenEndpointUrl` | *(Optional)* Identity provider token endpont used by Swagger UI to obtain token used to invoke API endpoints. If this endpoint is not available some OAuth flows might not be available in Swagger UI |
| `XmlCommentsFilePath` | *(Optional)* Path XML comments provides by project build. This information is used by Swagger generator to provide description of exposed models and properties. |

Full control over `SwaggerInitializer` can be obtained by defined child class. The following protected methods are avaible:

| Method | Description |
|---|---|
| `ConfigureSwaggerOptions` | This method is passed as parameter to `app.UseSwagger()`. This method configures location of Swagger document uri. |
| `ConfigureSwaggerUiOptions` | This method is passed as parameter to `app.UseSwaggerUI()`. This method configures route used by Swagger endpoint. |
| `ConfigureSwaggerGenOptions` | This method is passed as parameter to `services.AddSwaggerGen()`. It configures security definitions based on values defined in `Options` property. Additionally it adds support of annotations, injects security schemes to endpoint definitions and enables XML comments support. |

## Links

Current project is Swagger extension of Microservice Framework. You can find references to all available projects in core project of framework [GodelTech.Microservices.Core](https://github.com/GodelTech/GodelTech.Microservices.Core). Extension is based on [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) project.