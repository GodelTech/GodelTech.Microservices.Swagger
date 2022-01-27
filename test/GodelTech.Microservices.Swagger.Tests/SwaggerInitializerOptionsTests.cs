using System;
using System.Collections.Generic;
using Xunit;

namespace GodelTech.Microservices.Swagger.Tests
{
    public class SwaggerInitializerOptionsTests
    {
        private readonly SwaggerInitializerOptions _options;

        public SwaggerInitializerOptionsTests()
        {
            _options = new SwaggerInitializerOptions();
        }

        [Fact]
        public void DocumentTitle_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal("API", _options.DocumentTitle);
        }

        [Fact]
        public void DocumentTitle_Set_Success()
        {
            // Arrange
            var expectedResult = "Test DocumentTitle";

            // Act
            _options.DocumentTitle = expectedResult;

            // Assert
            Assert.Equal(expectedResult, _options.DocumentTitle);
        }

        [Fact]
        public void DocumentVersion_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal("v1", _options.DocumentVersion);
        }


        [Fact]
        public void DocumentVersion_Set_Success()
        {
            // Arrange
            var expectedResult = "Test DocumentVersion";

            // Act
            _options.DocumentVersion = expectedResult;

            // Assert
            Assert.Equal(expectedResult, _options.DocumentVersion);
        }

        [Fact]
        public void XmlCommentsFilePath_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Null(_options.XmlCommentsFilePath);
        }

        [Fact]
        public void XmlCommentsFilePath_Set_Success()
        {
            // Arrange
            var expectedResult = "Test XmlCommentsFilePath";

            // Act
            _options.XmlCommentsFilePath = expectedResult;

            // Assert
            Assert.Equal(expectedResult, _options.XmlCommentsFilePath);
        }

        [Fact]
        public void AuthorizationUrl_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Null(_options.AuthorizationUrl);
        }

        [Fact]
        public void AuthorizationUrl_Set_Success()
        {
            // Arrange
            var expectedResult = new Uri("http://test.dev");

            // Act
            _options.AuthorizationUrl = expectedResult;

            // Assert
            Assert.Equal(expectedResult, _options.AuthorizationUrl);
        }

        [Fact]
        public void TokenUrl_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Null(_options.TokenUrl);
        }

        [Fact]
        public void TokenUrl_Set_Success()
        {
            // Arrange
            var expectedResult = new Uri("http://test.dev");

            // Act
            _options.TokenUrl = expectedResult;

            // Assert
            Assert.Equal(expectedResult, _options.TokenUrl);
        }

        [Fact]
        public void Scopes_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(new Dictionary<string, string>(), _options.Scopes);
        }

        [Fact]
        public void Scopes_Set_Success()
        {
            // Arrange
            var expectedResult = new Dictionary<string, string>
            {
                {"TestKey", "TestValue"}
            };

            // Act
            _options.Scopes = expectedResult;

            // Assert
            Assert.Equal(expectedResult, _options.Scopes);
        }
    }
}