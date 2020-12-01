using GodelTech.Microservices.Security.Services;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace GodelTech.Microservices.Website
{
    public class PolicyFactory : IAuthorizationPolicyFactory
    {
        public IReadOnlyDictionary<string, AuthorizationPolicy> Create()
        {
            var policyBuilder = new AuthorizationPolicyBuilder();

            policyBuilder.RequireAuthenticatedUser();
            policyBuilder.RequireClaim("scope", "weather.add");

            return new Dictionary<string, AuthorizationPolicy>
            {
                ["add"] = policyBuilder.Build()
            };
        }
    }
}
