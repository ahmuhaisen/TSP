using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TPS.Infrastructure.Data;
using TPS.Infrastructure.Testing;

namespace TPS.WebAPI.Controllers;


[ApiController]
[Route("api/testing")]
public class TestsController(ApplicationDbContext _context) : ControllerBase
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
}
