namespace TPS.Application.Areas.Authentication.Contracts;
public record LoginResponse(
    string Token,
    string UserType,
    Guid Id,
    string fullName,
    string email,
    string profileImageId
);
