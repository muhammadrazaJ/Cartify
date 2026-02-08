using Cartify.Extensions;
using Cartify.Models;
using Cartify.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Controllers
{
    /// <summary>
    /// MVC controller for admin-only pages. Protected with [Authorize(Roles = "Admin")].
    /// Role is read from JWT claims only. 401 if no/invalid JWT; 403 if not Admin.
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminUserService _adminUserService;

        public AdminController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        /// <summary>
        /// GET /Admin — Admin dashboard.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            var role = User.GetRole();
            var userId = User.GetUserId();
            var email = User.GetUserEmail();

            ViewBag.UserId = userId;
            ViewBag.Email = email;
            ViewBag.Role = role;

            return View();
        }

        /// <summary>
        /// GET /Admin/Customers — Customer listing (Role = User). Admin-only.
        /// </summary>
        [HttpGet]
        public IActionResult Customers()
        {
            var customers = _adminUserService.GetCustomers();
            return View(customers);
        }

        /// <summary>
        /// GET /Admin/Customers/Details/{id} — View customer details.
        /// </summary>
        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var customer = _adminUserService.GetCustomerById(id);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        /// <summary>
        /// GET /Admin/Customers/ConfirmStatus — Confirmation page before activate/deactivate.
        /// </summary>
        [HttpGet]
        public IActionResult ConfirmStatus(string id, bool activate)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var customer = _adminUserService.GetCustomerById(id);
            if (customer == null)
                return NotFound();

            ViewBag.Activate = activate;
            return View(customer);
        }

        /// <summary>
        /// POST /Admin/Customers/Activate/{id} — Activate customer. Call after confirmation.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Activate(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var updated = _adminUserService.SetCustomerStatus(id, UserStatus.Active);
            if (!updated)
                return NotFound();

            TempData["Message"] = "Customer account has been activated.";
            return RedirectToAction(nameof(Details), new { id });
        }

        /// <summary>
        /// POST /Admin/Customers/Deactivate/{id} — Deactivate customer. Call after confirmation.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deactivate(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var updated = _adminUserService.SetCustomerStatus(id, UserStatus.Inactive);
            if (!updated)
                return NotFound();

            TempData["Message"] = "Customer account has been deactivated.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
