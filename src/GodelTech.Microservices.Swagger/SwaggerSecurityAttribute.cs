using System;

namespace GodelTech.Microservices.Swagger
{
    /// <summary>
    /// Swagger security attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class SwaggerSecurityAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerSecurityAttribute"/> class.
        /// </summary>
        /// <param name="scopes">Scopes.</param>
        public SwaggerSecurityAttribute(params string[] scopes)
        {
            Scopes = scopes;
        }

        /// <summary>
        /// Scopes.
        /// </summary>
        public string[] Scopes { get; }
    }
}
