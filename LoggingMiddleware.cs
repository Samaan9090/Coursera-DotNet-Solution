using Microsoft.AspNetCore.Http;
using System.Diagnostics;
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
            var stopwatch = Stopwatch.StartNew();

            // Log the incoming request
            _logger.LogInformation("Request starting HTTP {Method} {Path}", request.Method, request.Path);

            await _next(httpContext); // Call the next middleware

            stopwatch.Stop();

            // Log the response after the request is processed
            _logger.LogInformation("Request finished HTTP {Method} {Path} responded {StatusCode} in {ElapsedMilliseconds}ms", 
                request.Method, request.Path, httpContext.Response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
    }
}