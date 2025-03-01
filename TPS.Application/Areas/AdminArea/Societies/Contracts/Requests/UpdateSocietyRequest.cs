namespace TPS.Application.Areas.AdminArea.Societies.Contracts.Requests;

public class UpdateSocietyRequest
{
       public required string Name { get; set; }
       public required string Description { get; set; }
       public required string LogoBase64 { get; set; }
       public string? ThemeColor { get; set; }
}

//TODO: Add validation