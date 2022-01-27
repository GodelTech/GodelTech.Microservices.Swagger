using System;
using System.Collections.Generic;

namespace GodelTech.Microservices.Swagger.Tests.Fakes
{
    public static class SwaggerInitializerOptionsHelpers
    {
        public static SwaggerInitializerOptions CreateWithNullAuthorizationUrl()
        {
            return new SwaggerInitializerOptions
            {
                AuthorizationUrl = null,
                TokenUrl = new Uri("http://test.dev"),
                Scopes = new Dictionary<string, string>()
            };
        }

        public static SwaggerInitializerOptions CreateWithNullTokenUrl()
        {
            return new SwaggerInitializerOptions
            {
                AuthorizationUrl = new Uri("http://test.dev"),
                TokenUrl = null,
                Scopes = new Dictionary<string, string>()
            };
        }

        public static SwaggerInitializerOptions CreateWithNullScopes()
        {
            return new SwaggerInitializerOptions
            {
                AuthorizationUrl = new Uri("http://test.dev"),
                TokenUrl = new Uri("http://test.dev"),
                Scopes = null
            };
        }
    }
}