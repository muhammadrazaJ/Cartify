using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Cartify.Models;
using Microsoft.IdentityModel.Tokens;

namespace Cartify.Services
{
    /// <summary>
    /// Service for generating JWT tokens.
    /// Uses HMAC-SHA256 for signing with configurable secret from appsettings.
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        /// <inheritdoc />
        public string GenerateToken(string userId, string email, string fullName, string role)
        {
            // Create claims for the token (UserId, Email, Role as per requirements)
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, fullName),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token ID
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.GetSigningKey()));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_jwtSettings.ExpiryInHours),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
