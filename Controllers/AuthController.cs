using Cartify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Controllers
{
    /// <summary>
    /// MVC controller for authentication views.
    /// Serves Razor pages for Login and Register.
    /// Actual auth logic is in AuthApiController.
    /// </summary>
    [AllowAnonymous]
    public class AuthController : Controller
    {
        /// <summary>
        /// GET /Auth/Login - Displays the login form.
        /// </summary>
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
            return View(new LoginViewModel());
        }

        /// <summary>
        /// GET /Auth/Register - Displays the registration form.
        /// </summary>
        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
            return View(new RegisterViewModel());
        }

        /// <summary>
        /// POST /Auth/Logout - Clears JWT cookie and redirects to home.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token", new CookieOptions { Path = "/" });
            return RedirectToAction("Index", "Home");
        }
    }
}
