using Cartify.Models;

namespace Cartify.Services;

/// <summary>
/// Admin user/customer operations. Currently in-memory; later replace implementation with EF Core.
/// </summary>
public interface IAdminUserService
{
    /// <summary>
    /// Get all customers (Role = User). Later: query Users table via EF Core.
    /// </summary>
    IReadOnlyList<CustomerListDto> GetCustomers();

    /// <summary>
    /// Get customer by UserId. Later: single query by primary key.
    /// </summary>
    CustomerDetailDto? GetCustomerById(string userId);

    /// <summary>
    /// Set customer status (Active/Inactive). Later: update Users.Status in DB.
    /// </summary>
    bool SetCustomerStatus(string userId, UserStatus status);
}
