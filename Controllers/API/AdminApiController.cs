using Cartify.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Controllers.Api
{
    /// <summary>
    /// API controller for admin-only operations.
    /// Protected with [Authorize(Roles = "Admin")].
    /// Role is read from JWT claims only.
    /// </summary>
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminApiController : ControllerBase
    {
        /// <summary>
        /// GET /api/admin/dashboard
        /// Returns admin dashboard stats. Requires Admin role.
        /// Valid token + wrong role → 403 Forbidden.
        /// Missing/invalid token → 401 Unauthorized.
        /// </summary>
        [HttpGet("dashboard")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetDashboard()
        {
            // Example: Read role from JWT claims
            var role = User.GetRole();
            var userId = User.GetUserId();
            var email = User.GetUserEmail();

            return Ok(new
            {
                Message = "Admin dashboard data",
                UserId = userId,
                Email = email,
                Role = role,
                Stats = new
                {
                    TotalUsers = 0,
                    TotalOrders = 0
                }
            });
        }
    }
}
