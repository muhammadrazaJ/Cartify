using Cartify.Models;
using Cartify.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Controllers.Api
{
    /// <summary>
    /// Admin-only APIs for customer (User role) management.
    /// 401 if JWT missing/invalid; 403 if role is not Admin.
    /// No database calls; uses in-memory data. Replace with EF Core when DB is ready.
    /// </summary>
    [Route("api/admin/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminUsersController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;

        public AdminUsersController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        /// <summary>
        /// GET /api/admin/users/customers — Get all customers (Role = User).
        /// </summary>
        [HttpGet("customers")]
        [ProducesResponseType(typeof(IEnumerable<CustomerListDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCustomers()
        {
            var customers = _adminUserService.GetCustomers();
            return Ok(customers);
        }

        /// <summary>
        /// GET /api/admin/users/customers/{userId} — Get customer details by UserId.
        /// </summary>
        [HttpGet("customers/{userId}")]
        [ProducesResponseType(typeof(CustomerDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetCustomerById(string userId)
        {
            var customer = _adminUserService.GetCustomerById(userId);
            if (customer == null)
                return NotFound();
            return Ok(customer);
        }

        /// <summary>
        /// POST /api/admin/users/customers/{userId}/activate — Activate customer account.
        /// </summary>
        [HttpPost("customers/{userId}/activate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult ActivateCustomer(string userId)
        {
            var updated = _adminUserService.SetCustomerStatus(userId, UserStatus.Active);
            if (!updated)
                return NotFound();
            return NoContent();
        }

        /// <summary>
        /// POST /api/admin/users/customers/{userId}/deactivate — Deactivate customer account.
        /// </summary>
        [HttpPost("customers/{userId}/deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DeactivateCustomer(string userId)
        {
            var updated = _adminUserService.SetCustomerStatus(userId, UserStatus.Inactive);
            if (!updated)
                return NotFound();
            return NoContent();
        }
    }
}
