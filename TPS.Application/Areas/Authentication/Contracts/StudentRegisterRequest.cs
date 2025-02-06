using TSP.Domain.Entities;

namespace TPS.Application.Areas.Authentication.Contracts;

public class StudentRegisterRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Gender Gender { get; set; }
    public int DepartmentId { get; set; }
    public required string UniversityNumber { get; set; }
}
