using Microsoft.AspNetCore.Mvc;
using TPS.Infrastructure.Data;
using TPS.Infrastructure.Data.DataGenerators;
using TPS.Infrastructure.Emailing;
using TSP.Domain.Enums;

namespace TSP.WebAPI.Controllers;


[ApiController]
[Route($"api/[controller]")]
public class TestsController(ApplicationDbContext _context, IEmailService emailService, IGitHubService gitHubService) : ControllerBase
{
    [HttpPost("GenerateFakeData")]
    public async Task<IActionResult> GenerateFakeData(int number = 5)
    {
        var data = new FacultyMemberFaker().Generate(number);

        foreach (var row in data)
        {
            _context.Add(row);
        }

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("emails/send")]
    public async Task<IActionResult> SendFakeEmail(string subject)
    {
        try
        {
            await emailService.SendWelcomingEmail("ahmuhaisen03@gmail.com", "Ahmad Muhaisen", UserType.FacultyMember);
        }
        catch (Exception ex)
        {
            throw;
        }

        return Ok();
    }

   
}
