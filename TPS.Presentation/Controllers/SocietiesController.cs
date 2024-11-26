using Microsoft.AspNetCore.Mvc;
using TPS.Infrastructure.Data;
using TSP.Domain.Entities;

namespace TPS.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SocietiesController : ControllerBase
{

    public ApplicationDbContext Context { get; }

    public SocietiesController(ApplicationDbContext context)
    {
        Context = context;
    }


    [HttpPost]
    public IActionResult Post()
    {
        var society = new Society
        {
            ID = Guid.NewGuid(),
            Name = "Waves",
            Description = "Afia3 Robotics team",
            LogoID = "Majd.png",
            CreationDate = DateOnly.FromDateTime(DateTime.Now),
            ThemeColor = "#ffffff"
        };

        Context.Societies.Add(society);

        if (Context.SaveChanges() <= 0)
            return BadRequest("Error while creating the entity");

        return Ok(society);
    }
}
