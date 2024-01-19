using System;
using System.Reflection;

namespace GodelTech.Microservices.Swagger.Utilities
{
    internal class SystemVersion : IVersion
    {
        private readonly Func<Assembly> _getEntryAssembly;

        public SystemVersion(Func<Assembly> getEntryAssembly = default)
        {
            _getEntryAssembly = getEntryAssembly ?? Assembly.GetEntryAssembly;
        }

        public string GetVersion(string defaultVersion)
        {
            return _getEntryAssembly()?.GetName().Version?.ToString()
                   ?? defaultVersion;
        }
    }
}
