using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.OpenApi.Models;

namespace GodelTech.Microservices.Swagger.Tests.Fakes;

public class OpenApiSecuritySchemeComparer : IEqualityComparer<OpenApiSecurityScheme>
{
    public bool Equals(OpenApiSecurityScheme x, OpenApiSecurityScheme y)
    {
        // Check whether the compared objects reference the same data
        if (ReferenceEquals(x, y)) return true;

        // Check whether any of the compared objects is null
        if (x is null || y is null) return false;

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
        // Check whether the object is null
        if (obj is null) return 0;

        // Calculate the hash code for the object.
        return obj.Name.GetHashCode(StringComparison.InvariantCulture)
               ^ obj.Type.GetHashCode()
               ^ obj.In.GetHashCode()
               ^ obj.Scheme.GetHashCode(StringComparison.InvariantCulture)
               ^ obj.BearerFormat.GetHashCode(StringComparison.InvariantCulture)
               ^ obj.Description.GetHashCode(StringComparison.InvariantCulture);
    }
}
