﻿using System;

namespace GodelTech.Microservices.Swagger.Swagger
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class SwaggerRequiredScopesAttribute : Attribute
    {
        public string[] Scopes { get; }

        public SwaggerRequiredScopesAttribute(params string[] scopes)
        {
            Scopes = scopes ?? throw new ArgumentNullException(nameof(scopes));
        }
    }
}