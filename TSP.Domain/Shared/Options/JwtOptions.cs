namespace TSP.Domain.Shared.Options;

public class JwtOptions
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string Key { get; set; } = null!;
    public int ExpiryMinutes { get; set; }
}
