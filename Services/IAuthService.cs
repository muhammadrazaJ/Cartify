using Cartify.Models;

namespace Cartify.Services
{
    /// <summary>
    /// Interface for authentication operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Registers a new user with hashed password.
        /// </summary>
        /// <param name="model">Registration data</param>
        /// <returns>Result with success status and message</returns>
        Task<RegisterResponseDto> RegisterAsync(RegisterViewModel model);

        /// <summary>
        /// Validates credentials and returns login result with JWT token.
        /// </summary>
        /// <param name="model">Login credentials</param>
        /// <returns>Result with token and user info on success</returns>
        Task<LoginResponseDto> LoginAsync(LoginViewModel model);
    }
}
