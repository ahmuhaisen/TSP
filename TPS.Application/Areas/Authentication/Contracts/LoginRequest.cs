namespace TPS.Application.Areas.Authentication.Contracts;

// Shared DTOs
public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
