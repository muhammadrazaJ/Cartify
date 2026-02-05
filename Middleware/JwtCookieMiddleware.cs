namespace Cartify.Middleware
{
    /// <summary>
    /// Middleware that forwards JWT from auth_token cookie to Authorization header.
    /// Enables JWT Bearer authentication for browser requests (Razor views)
    /// where the token is stored in HttpOnly cookie instead of header.
    /// </summary>
    public class JwtCookieMiddleware
    {
        private readonly RequestDelegate _next;
        private const string AuthCookieName = "auth_token";
        private const string AuthHeaderScheme = "Bearer";

        public JwtCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // If no Authorization header but we have auth cookie, add the token to header
            if (!context.Request.Headers.ContainsKey("Authorization") &&
                context.Request.Cookies.TryGetValue(AuthCookieName, out var token) &&
                !string.IsNullOrWhiteSpace(token))
            {
                context.Request.Headers.Append("Authorization", $"{AuthHeaderScheme} {token}");
            }

            await _next(context);
        }
    }

    /// <summary>
    /// Extension method for registering JwtCookieMiddleware.
    /// </summary>
    public static class JwtCookieMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtCookieForwarding(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtCookieMiddleware>();
        }
    }
}
