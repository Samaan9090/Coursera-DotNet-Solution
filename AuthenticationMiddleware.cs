using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace UserManagementAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token) || !ValidateToken(token))
            {
                _logger.LogWarning("Unauthorized access attempt.");
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(new { error = "Unauthorized access." });
                return;
            }

            await _next(httpContext); // Token is valid, proceed to the next middleware
        }

        private bool ValidateToken(string token)
        {
            // Placeholder logic for token validation (replace with actual logic)
            return token == "valid-token"; // Simple example
        }
    }
}