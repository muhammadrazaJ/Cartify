using Cartify.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Controllers.Api
{
    /// <summary>
    /// API controller for authenticated customer/user operations.
    /// Protected with [Authorize] - any valid JWT token (User or Admin) can access.
    /// No role restriction for basic user routes.
    /// </summary>
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserApiController : ControllerBase
    {
        /// <summary>
        /// GET /api/user/profile
        /// Returns current user profile from JWT claims.
        /// Requires valid JWT token.
        /// </summary>
        [HttpGet("profile")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetProfile()
        {
            var userId = User.GetUserId();
            var email = User.GetUserEmail();
            var role = User.GetRole();

            return Ok(new
            {
                UserId = userId,
                Email = email,
                Role = role
            });
        }

        /// <summary>
        /// GET /api/user/orders
        /// Placeholder for user orders. Requires valid JWT.
        /// </summary>
        [HttpGet("orders")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetOrders()
        {
            var userId = User.GetUserId();
            return Ok(new
            {
                UserId = userId,
                Orders = Array.Empty<object>()
            });
        }
    }
}
