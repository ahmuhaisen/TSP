namespace TPS.Application.Common.Contracts.Authentication;

// Shared DTOs
public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
