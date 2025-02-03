using Microsoft.AspNetCore.Mvc;
using TPS.Application.Abstractions;
using TPS.Infrastructure.Data;
using TPS.Infrastructure.DataGenerators;

namespace TSP.WebAPI.Controllers;


[ApiController]
[Route($"api/[controller]")]
public class TestsController(ApplicationDbContext _context, IEmailService emailService, IGitHubService gitHubService) : ControllerBase
{
    [HttpPost("GenerateFakeData")]
    public async Task<IActionResult> GenerateFakeData()
    {
        var data = new FacultyMemberFaker().Generate(10);

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

    [HttpPost("upload-image")]
    public async Task<IActionResult> UploadImage([FromBody] string base64StringImage)
    {
        string base64Image = base64StringImage;
        var result = await gitHubService.uploadFile_EXPERIMENTAL("images", base64Image);

        if (result.IsSuccess)
        {
            Console.WriteLine($"Image uploaded successfully. Image ID: {result.Data}");
        }
        else
        {
            Console.WriteLine($"Error uploading image: {result.Error.Message}");
        }

        return Ok();
    }
}
