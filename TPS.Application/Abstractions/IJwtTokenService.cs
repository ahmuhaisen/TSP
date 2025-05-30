using TSP.Domain.Entities;

namespace TPS.Application.Abstractions;

public interface IJwtTokenService
{
    Task<string> GenerateAsync(ApplicationUser user, string? userRole = null);
    Task<string> GenerateResetAsync(ApplicationUser user, string? userRole = null);
}
