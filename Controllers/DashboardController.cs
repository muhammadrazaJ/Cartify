using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Controllers
{
    /// <summary>
    /// Protected dashboard controller - requires JWT authentication.
    /// Demonstrates [Authorize] attribute usage for protected routes.
    /// </summary>
    [Authorize]
    public class DashboardController : Controller
    {
        /// <summary>
        /// GET /Dashboard - Protected home/dashboard page.
        /// Redirects to login if not authenticated.
        /// </summary>
        public IActionResult Index()
        {
            ViewData["UserName"] = User.Identity?.Name ?? User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value ?? "User";
            return View();
        }
    }
}
