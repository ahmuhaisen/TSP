namespace TPS.Application.Areas.Shared.Profiles.Contracts;

public record UserProfileDto
{
    public Guid Id { get; init; }
    public string? ProfileImageId { get; set; }
    public string? Number { get; set; }
    public string? FullName { get; init; }
    public string? userType { get; init; }
    public string? Email { get; init; }
    public string? Department { get; init; }
    public string? School { get; init; }

    public List<MembershipBasicDetailsDto> Memberships { get; init; } = [];
}

public record MembershipBasicDetailsDto
{
    public string? SocietyName { get; set; }
    public string? SocietyLogoId { get; set; }
    public string? Section { get; set; }
    public DateOnly JoinDate { get; set; }
}

public record CurrentUserDto
{
    public Guid Id { get; init; }
    public string? ProfileImageId { get; set; }
    public string Number { get; set; } = default!;
    public string FullName { get; init; } = default!;
    public string userType { get; init; } = default!;
    public string Email { get; init; } = default!;
    public int DepartmentId { get; init; } = default!;

}