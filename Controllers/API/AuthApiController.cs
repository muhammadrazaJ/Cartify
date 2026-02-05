using Cartify.Models;
using Cartify.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Controllers.Api
{
    /// <summary>
    /// API controller for authentication endpoints.
    /// Handles registration and login, returns JWT token on successful login.
    /// </summary>
    // Explicit route so endpoints match frontend calls: /api/auth/...
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthApiController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        /// <summary>
        /// POST /api/auth/register
        /// Registers a new user. Password is hashed before saving.
        /// Prevents duplicate email registration.
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (model == null)
            {
                return BadRequest(new RegisterResponseDto { Success = false, Message = "Invalid request data." });
            }

            var result = await _authService.RegisterAsync(model);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// POST /api/auth/login
        /// Validates credentials and returns JWT token on success.
        /// Token contains UserId, Email, and Role claims. Expiry: 1 hour.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (model == null)
            {
                return BadRequest(new LoginResponseDto { Success = false, Message = "Invalid request data." });
            }

            var result = await _authService.LoginAsync(model);

            if (!result.Success)
            {
                return Unauthorized(result);
            }

            // Store JWT securely in HttpOnly cookie for web app
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = !HttpContext.Request.Host.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase),
                SameSite = SameSiteMode.Strict,
                MaxAge = TimeSpan.FromHours(_configuration.GetValue<int>("Jwt:ExpiryInHours", 1)),
                Path = "/"
            };
            Response.Cookies.Append("auth_token", result.Token!, cookieOptions);

            return Ok(result);
        }

        /// <summary>
        /// POST /api/auth/logout
        /// Clears the auth token cookie.
        /// </summary>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token", new CookieOptions { Path = "/" });
            return Ok(new { Success = true, Message = "Logged out successfully." });
        }
    }
}
