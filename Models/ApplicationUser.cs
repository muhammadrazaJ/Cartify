using Microsoft.AspNetCore.Identity;

namespace Cartify.Models
{
    /// <summary>
    /// Application user model extending IdentityUser.
    /// Adds FullName and uses built-in Identity for password hashing and role management.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// User's full name for display purposes.
        /// </summary>
        public string FullName { get; set; } = string.Empty;
    }
}
