namespace Cartify.Models;

/// <summary>
/// Status for user account (ERD: Users.Status).
/// When connected to DB, this can map to a lookup or enum column.
/// </summary>
public enum UserStatus
{
    Active = 0,
    Inactive = 1
}

/// <summary>
/// DTO for customer list API/view.
/// Maps from ERD: Users (UserId, FullName, Email, PhoneNumber, Role, Status, CreatedAt).
/// </summary>
public class CustomerListDto
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "User";
    public UserStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO for customer detail API/view.
/// Same as list but can be extended with extra fields when DB is connected.
/// </summary>
public class CustomerDetailDto
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "User";
    public UserStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Internal DTO for in-memory store. Mirrors ERD Users table.
/// Replace with EF Core entity when database is implemented.
/// </summary>
public class UserDto
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string Role { get; set; } = "User";
    public UserStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
