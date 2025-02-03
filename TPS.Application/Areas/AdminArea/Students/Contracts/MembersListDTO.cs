namespace TPS.Application.Areas.AdminArea.Students.Contracts;
public class MembersListDTO
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Position { get; set; }
    public DateOnly JoinDate { get; set; }
}
