namespace Cartify.Models
{
    /// <summary>
    /// Configuration model for JWT settings from appsettings.json.
    /// </summary>
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpiryInHours { get; set; } = 1;
    }
}
