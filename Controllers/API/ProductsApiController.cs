using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cartify.Controllers.Api
{
    /// <summary>
    /// API controller for product endpoints.
    /// Public access - [AllowAnonymous] for listing and details.
    /// </summary>
    [Route("api/products")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsApiController : ControllerBase
    {
        /// <summary>
        /// GET /api/products - Product listing (temporary placeholder).
        /// Public - no authentication required.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult GetProducts()
        {
            return Ok(new
            {
                Products = Array.Empty<object>(),
                Message = "Product listing placeholder - ERD to be implemented"
            });
        }

        /// <summary>
        /// GET /api/products/{id} - Product details (temporary placeholder).
        /// Public - no authentication required.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetProduct(int id)
        {
            return Ok(new
            {
                Id = id,
                Name = "Product placeholder",
                Message = "Product details - ERD to be implemented"
            });
        }
    }
}
