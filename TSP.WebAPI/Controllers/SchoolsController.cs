using Microsoft.AspNetCore.Mvc;
using MediatR;
using TPS.Application.Schools.Queries;

namespace TSP.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolsController : ApiController
{
    public SchoolsController(ISender sender) : base(sender)
    {}


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var query = new GetAllSchoolsWithDepartments.Query();

        var task = _sender.Send(query);

        return await FromResult(task);
    }
}
