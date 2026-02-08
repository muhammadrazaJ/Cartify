namespace Cartify.Models
{
    /// <summary>
    /// Configuration model for JWT settings from appsettings.json.
    /// Supports both Key and SecretKey for signing (Key preferred for validation).
    /// </summary>
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        /// <summary>Signing key for JWT validation (Jwt:Key).</summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>Alternative signing key (Jwt:SecretKey) - used for token generation if Key is empty.</summary>
        public string SecretKey { get; set; } = string.Empty;

        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryInHours { get; set; } = 1;

        /// <summary>Gets the effective signing key (Key or SecretKey fallback).</summary>
        public string GetSigningKey() => !string.IsNullOrEmpty(Key) ? Key : SecretKey;
    }
}
