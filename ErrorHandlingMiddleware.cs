using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace UserManagementAPI.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); // Call the next middleware
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "Unhandled exception occurred.");

                // Return a consistent error response
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var errorResponse = new { error = "Internal server error." };
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}