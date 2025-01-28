using TSP.Domain.Entities;

namespace TPS.Application.Abstractions;

public interface ITokenService
{
    Task<string> GenerateAsync(ApplicationUser user);
}
