using Cartify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cartify.Services
{
    /// <summary>
    /// Authentication service handling registration and login.
    /// Uses ASP.NET Core Identity for password hashing and user management.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtTokenService _jwtTokenService;
        private const string DefaultRole = "User";

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtTokenService jwtTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
        }

        /// <inheritdoc />
        public async Task<RegisterResponseDto> RegisterAsync(RegisterViewModel model)
        {
            // Check for duplicate email
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                return new RegisterResponseDto
                {
                    Success = false,
                    Message = "An account with this email already exists."
                };
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                EmailConfirmed = true // Skip email confirmation for simpler flow
            };

            // Identity hashes password automatically before saving
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                return new RegisterResponseDto
                {
                    Success = false,
                    Message = errors
                };
            }

            // Assign default role
            await _userManager.AddToRoleAsync(user, DefaultRole);

            return new RegisterResponseDto
            {
                Success = true,
                Message = "Registration successful. You can now log in.",
                UserId = user.Id
            };
        }

        /// <inheritdoc />
        public async Task<LoginResponseDto> LoginAsync(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            // Validate password using Identity's secure password verification
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password."
                };
            }

            // Get user role (first role or default)
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? DefaultRole;

            // Generate JWT token only on successful login
            var token = _jwtTokenService.GenerateToken(
                user.Id,
                user.Email ?? string.Empty,
                user.FullName,
                role
            );

            return new LoginResponseDto
            {
                Success = true,
                Token = token,
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Role = role,
                Message = "Login successful."
            };
        }
    }
}
