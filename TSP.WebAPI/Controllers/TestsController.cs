using Microsoft.AspNetCore.Mvc;
using TPS.Application.Abstractions;
using TPS.Infrastructure.Data;
using TPS.Infrastructure.Data.DataGenerators;

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
    public async Task<IActionResult> SendFakeEmail()
    {
        try
        {
            await emailService.Send("ahmuhaisen03@gmail.com", "Hi Suhaib", "This is a test email");
        }
        catch (Exception ex)
        {
            throw;
        }

        return Ok();
    }

   
}
