namespace TPS.Application.Areas.AdminArea.Statistics.Contracts;

public class SocietyCountDTO
{
    public Guid id {  get; set; }
    public required string Name { get; set; }
    public int count { get; set; }
}
