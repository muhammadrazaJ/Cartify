using System.Security.Claims;

namespace Cartify.Extensions
{
    /// <summary>
    /// Extension methods for reading claims from JWT-authenticated user.
    /// Roles are read ONLY from JWT claims (ClaimTypes.Role).
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Gets the current user's role from JWT claims.
        /// Returns null if not authenticated or no role claim.
        /// </summary>
        public static string? GetRole(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value;
        }

        /// <summary>
        /// Checks if the user has the specified role (from JWT claims).
        /// </summary>
        public static bool IsInRoleFromJwt(this ClaimsPrincipal user, string role)
        {
            return user.GetRole()?.Equals(role, StringComparison.OrdinalIgnoreCase) == true;
        }

        /// <summary>
        /// Gets the user ID (NameIdentifier) from JWT claims.
        /// </summary>
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        /// <summary>
        /// Gets the user email from JWT claims.
        /// </summary>
        public static string? GetUserEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }
    }
}
