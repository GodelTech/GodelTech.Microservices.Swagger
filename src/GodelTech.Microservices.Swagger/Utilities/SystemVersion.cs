using System.Reflection;

namespace GodelTech.Microservices.Swagger.Utilities
{
    internal class SystemVersion : IVersion
    {
        public string GetVersion(string defaultVersion)
        {
            return Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? defaultVersion;
        }
    }
}
