using DexTranslate.Api.MiddleWare;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DexTranslate.ApiFixtures.Middleware
{
    public class ErrorHandlingMiddlewareFixtures
    {
        [Fact]
        public async Task It_Returns_Error_Response()
        {
            // Arrange
            DefaultHttpContext context = CreateHttpContext();
            var middleware = new ErrorHandlingMiddleware((c) => throw new ApplicationException("Test error"));

            // Act
            await middleware.Invoke(context);

            // Assert
            Assert.Equal("application/json", context.Response.ContentType);
            Assert.Equal("{\"statusCode\":500,\"message\":\"Test error\"}", ReadResponseBody(context));
        }

        private static DefaultHttpContext CreateHttpContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            return context;
        }

        private static string ReadResponseBody(DefaultHttpContext context)
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(context.Response.Body))
            {
                return reader.ReadToEnd();
            }
        }
    }
}