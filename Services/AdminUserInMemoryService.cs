using Cartify.Models;

namespace Cartify.Services;

/// <summary>
/// In-memory implementation for admin user management. No database.
/// Replace with a service that uses ApplicationDbContext/Users table when EF Core is wired.
/// </summary>
public class AdminUserInMemoryService : IAdminUserService
{
    // In-memory store matching ERD: UserId, FullName, Email, PhoneNumber, Role, Status, CreatedAt
    // TODO: Replace with DbSet&lt;User&gt; or repository when DB is ready
    private static readonly List<UserDto> _users = new()
    {
        new UserDto
        {
            UserId = "cust-001",
            FullName = "Alice Johnson",
            Email = "alice@example.com",
            PhoneNumber = "+1234567890",
            Role = "User",
            Status = UserStatus.Active,
            CreatedAt = new DateTime(2025, 1, 15, 10, 0, 0, DateTimeKind.Utc)
        },
        new UserDto
        {
            UserId = "cust-002",
            FullName = "Bob Smith",
            Email = "bob@example.com",
            PhoneNumber = "+1987654321",
            Role = "User",
            Status = UserStatus.Inactive,
            CreatedAt = new DateTime(2025, 2, 1, 14, 30, 0, DateTimeKind.Utc)
        },
        new UserDto
        {
            UserId = "cust-003",
            FullName = "Carol Williams",
            Email = "carol@example.com",
            PhoneNumber = null,
            Role = "User",
            Status = UserStatus.Active,
            CreatedAt = new DateTime(2025, 2, 5, 9, 0, 0, DateTimeKind.Utc)
        }
    };

    public IReadOnlyList<CustomerListDto> GetCustomers()
    {
        // Filter by Role = User (customers only). Later: .Where(u => u.Role == "User")
        return _users
            .Where(u => string.Equals(u.Role, "User", StringComparison.OrdinalIgnoreCase))
            .Select(MapToListDto)
            .ToList();
    }

    public CustomerDetailDto? GetCustomerById(string userId)
    {
        var user = _users.FirstOrDefault(u =>
            string.Equals(u.UserId, userId, StringComparison.OrdinalIgnoreCase));
        if (user == null) return null;
        return MapToDetailDto(user);
    }

    public bool SetCustomerStatus(string userId, UserStatus status)
    {
        var user = _users.FirstOrDefault(u =>
            string.Equals(u.UserId, userId, StringComparison.OrdinalIgnoreCase));
        if (user == null) return false;
        user.Status = status;
        return true;
    }

    private static CustomerListDto MapToListDto(UserDto u)
    {
        return new CustomerListDto
        {
            UserId = u.UserId,
            FullName = u.FullName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Role = u.Role,
            Status = u.Status,
            CreatedAt = u.CreatedAt
        };
    }

    private static CustomerDetailDto MapToDetailDto(UserDto u)
    {
        return new CustomerDetailDto
        {
            UserId = u.UserId,
            FullName = u.FullName,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Role = u.Role,
            Status = u.Status,
            CreatedAt = u.CreatedAt
        };
    }
}
