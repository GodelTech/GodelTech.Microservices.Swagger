using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Models;

namespace GodelTech.Microservices.Swagger.Tests.Fakes
{
    public class OpenApiSecuritySchemeComparer : IEqualityComparer<OpenApiSecurityScheme>
    {
        public bool Equals(OpenApiSecurityScheme x, OpenApiSecurityScheme y)
        {
            // Check whether the compared objects reference the same data
            if (ReferenceEquals(x, y)) return true;

            // Check whether any of the compared objects is null
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            // Check whether the objects' properties are equal.
            return x.Name == y.Name
                   && x.Type == y.Type
                   && x.In == y.In
                   && x.Scheme == y.Scheme
                   && x.BearerFormat == y.BearerFormat
                   && x.Description == y.Description;
        }

        public int GetHashCode([DisallowNull] OpenApiSecurityScheme obj)
        {
            // Calculate the hash code for the object.
            return obj.Name.GetHashCode()
                   ^ obj.Type.GetHashCode()
                   ^ obj.In.GetHashCode()
                   ^ obj.Scheme.GetHashCode()
                   ^ obj.BearerFormat.GetHashCode()
                   ^ obj.Description.GetHashCode();
        }
    }
}