using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Xunit;

namespace GodelTech.Microservices.Swagger.Tests
{
    public class SwaggerSecurityAttributeTests
    {
        public static IEnumerable<object[]> ScopesMemberData =>
            new Collection<object[]>
            {
                new object[]
                {
                    null
                },
                new object[]
                {
                    Array.Empty<string>()
                },
                new object[]
                {
                    new string[]
                    {
                        "first scope"
                    }
                },
                new object[]
                {
                    new string[]
                    {
                        "first scope",
                        "second scope",
                    }
                }
            };

        [Fact]
        public void Class_HasAttributeUsageAttribute()
        {
            // Arrange & Act
            var attribute = typeof(SwaggerSecurityAttribute).GetCustomAttribute<AttributeUsageAttribute>(false);

            // Assert
            Assert.NotNull(attribute);
            Assert.Equal(AttributeTargets.Class | AttributeTargets.Method, attribute.ValidOn);
        }

        [Fact]
        public void Constructor_Success()
        {
            // Arrange & Act
            var attribute = new SwaggerSecurityAttribute();

            // Assert
            Assert.Empty(attribute.Scopes);
        }

        [Theory]
        [MemberData(nameof(ScopesMemberData))]
        public void Scopes_Get_Success(string[] scopes)
        {
            // Arrange & Act
            var attribute = new SwaggerSecurityAttribute(scopes);

            // Assert
            Assert.Equal(scopes, attribute.Scopes);
        }
    }
}