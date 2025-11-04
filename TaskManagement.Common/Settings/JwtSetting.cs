namespace TaskManagement.Common.Settings;
public class JwtSetting
{
    public required string SecretKey { get; set; }
    public required string Encryptkey { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public int NotBeforeMinutes { get; set; }
    public int ExpirationMinutesAccessToken { get; set; }
    public int ExpirationDaysRefreshToken { get; set; } 
}
