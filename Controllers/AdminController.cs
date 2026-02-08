using Cartify.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Controllers
{
    /// <summary>
    /// MVC controller for admin-only pages.
    /// Protected with [Authorize(Roles = "Admin")].
    /// Role is read from JWT claims only.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        /// <summary>
        /// GET /Admin - Admin dashboard.
        /// Requires Admin role from JWT.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            // Example: Read role from JWT claims
            var role = User.GetRole();
            var userId = User.GetUserId();
            var email = User.GetUserEmail();

            ViewBag.UserId = userId;
            ViewBag.Email = email;
            ViewBag.Role = role;

            return View();
        }
    }
}
