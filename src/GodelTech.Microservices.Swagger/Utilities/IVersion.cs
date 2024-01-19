namespace GodelTech.Microservices.Swagger.Utilities
{
    /// <summary>
    /// Version.
    /// </summary>
    public interface IVersion
    {
        /// <summary>
        /// Gets version.
        /// </summary>
        /// <param name="defaultVersion">The default version value.</param>
        /// <returns>Version.</returns>
        string GetVersion(string defaultVersion);
    }
}
