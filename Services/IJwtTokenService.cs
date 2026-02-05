using System.Security.Claims;

namespace Cartify.Services
{
    /// <summary>
    /// Interface for JWT token generation service.
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generates a JWT token with the specified claims.
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <param name="email">User email</param>
        /// <param name="fullName">User full name</param>
        /// <param name="role">User role</param>
        /// <returns>Generated JWT token string</returns>
        string GenerateToken(string userId, string email, string fullName, string role);
    }
}
