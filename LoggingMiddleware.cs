using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace UserManagementAPI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var request = httpContext.Request;

            // Log the incoming request
            _logger.LogInformation("Request: {Method} {Path}", request.Method, request.Path);

            // Create a copy of the response body to log after processing the request
            var originalBodyStream = httpContext.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                httpContext.Response.Body = memoryStream;

                await _next(httpContext); // Call the next middleware

                // Log the response after the request is processed
                _logger.LogInformation("Response: {StatusCode}", httpContext.Response.StatusCode);

                // Copy the response body back to the original stream
                await memoryStream.CopyToAsync(originalBodyStream);
            }
        }
    }
}