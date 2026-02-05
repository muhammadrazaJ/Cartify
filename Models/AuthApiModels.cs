namespace Cartify.Models
{
    /// <summary>
    /// DTO for API login response containing JWT token and user info.
    /// </summary>
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public string? Message { get; set; }
    }

    /// <summary>
    /// DTO for API registration response.
    /// </summary>
    public class RegisterResponseDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? UserId { get; set; }
    }
}
