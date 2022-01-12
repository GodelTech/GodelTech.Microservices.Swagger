using System;
using System.Collections.Generic;

namespace GodelTech.Microservices.Swagger
{
    /// <summary>
    /// Swagger initializer options.
    /// </summary>
    public sealed class SwaggerInitializerOptions
    {
        /// <summary>
        /// The title of the application.
        /// </summary>
        public string DocumentTitle { get; set; } = "API";

        /// <summary>
        /// REQUIRED. The version of the OpenAPI document.
        /// </summary>
        public string DocumentVersion { get; set; } = "v1";

        /// <summary>
        /// An absolute path to the file that contains XML Comments.
        /// </summary>
        public string XmlCommentsFilePath { get; set; }

        /// <summary>
        /// The authorization URL to be used for this flow.
        /// Applies to implicit and authorizationCode OAuthFlow.
        /// </summary>
        public Uri AuthorizationUrl { get; set; }

        /// <summary>
        /// The token URL to be used for this flow.
        /// Applies to password, clientCredentials, and authorizationCode OAuthFlow.
        /// </summary>
        public Uri TokenUrl { get; set; }

        /// <summary>
        /// A map between the scope name and a short description for it.
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
                               // You can suppress the warning if the property is part of a Data Transfer Object (DTO) class.
        public IDictionary<string, string> Scopes { get; set; } = new Dictionary<string, string>();
#pragma warning restore CA2227 // Collection properties should be read only
    }
}
