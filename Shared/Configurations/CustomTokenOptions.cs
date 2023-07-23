namespace Shared.Configurations;

public class CustomTokenOptions
{
    public string Issuer { get; set; }
    public List<string> Audiences { get; set; }
    public int AccessTokenExpiration { get; set; }
    public int RefreshTokenExpiration { get; set; }
    public string SecurityKey { get; set; }
}