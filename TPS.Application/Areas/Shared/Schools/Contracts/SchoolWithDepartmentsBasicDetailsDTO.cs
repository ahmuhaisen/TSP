namespace TPS.Application.Areas.Shared.Schools.Contracts;

public class SchoolWithDepartmentsBasicDetailsDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public List<DepartmentBasicDetailsDTO> Departments { get; set; } = new();
}

public class DepartmentBasicDetailsDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}
