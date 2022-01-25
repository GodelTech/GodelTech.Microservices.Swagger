namespace GodelTech.Microservices.Swagger
{
    /// <summary>
    /// OAuth2 constants.
    /// </summary>
    public static class OAuth2Security
    {
        /// <summary>
        /// OAuth2.
        /// </summary>
        public static readonly string OAuth2 = "oauth2";

        /// <summary>
        /// Authorization code.
        /// </summary>
        public static readonly string AuthorizationCode = "oauth2-authorization-code";

        /// <summary>
        /// Client credentials.
        /// </summary>
        public static readonly string ClientCredentials = "oauth2-client-credentials";

        /// <summary>
        /// Resource owner password credentials.
        /// </summary>
        public static readonly string ResourceOwnerPasswordCredentials = "oauth2-password";

        /// <summary>
        /// Implicit.
        /// </summary>
        public static readonly string Implicit = "oauth2-implicit";
    }
}
