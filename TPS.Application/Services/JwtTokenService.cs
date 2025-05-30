using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TPS.Application.Abstractions;
using TSP.Domain.Entities;
using TSP.Domain.Shared.Options;

namespace TPS.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IOptions<JwtOptions> _jwtOptions;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtTokenService(
        IOptions<JwtOptions> jwtOptions,
        UserManager<ApplicationUser> userManager)
    {
        _jwtOptions = jwtOptions;
        _userManager = userManager;
    }

    private async Task<List<Claim>> generateClaims(ApplicationUser user, string? userRole = null)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Name, $"{user.FirstName} {user.LastName}"),
            new Claim("uid", user.Id.ToString()),
            new Claim("pid", user.ProfileImageId ?? string.Empty),
            new Claim("pic", user.ProfileImageId ?? string.Empty),
        };
        if (userRole != null)
            claims.Add(new Claim("role", userRole));
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim("rle", role)));
        return claims;

    }
    private async Task<SigningCredentials> GenerateCreds()
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtOptions.Value.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        return creds;
    }

    public async Task<string> GenerateAsync(ApplicationUser user, string? userRole = null)
    {
        var claims = generateClaims(user,userRole).Result;
        var creds = GenerateCreds().Result;

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Value.Issuer,
            audience: _jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(_jwtOptions.Value.ExpiryMinutes)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<string> GenerateResetAsync(ApplicationUser user, string? userRole = null)
    {
        var claims = generateClaims(user, userRole).Result;
        var creds = GenerateCreds().Result;
        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Value.Issuer,
            audience: _jwtOptions.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
