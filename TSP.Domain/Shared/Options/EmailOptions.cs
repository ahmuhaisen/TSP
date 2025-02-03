namespace TSP.Domain.Shared.Options;

public class EmailOptions
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
}
