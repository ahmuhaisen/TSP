using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.Shared.Schools.Queries;
using TSP.WebAPI.Controllers;


[ApiController]
[Route("api/[controller]")]
public class RanksController : ApiController
{
    public RanksController(ISender sender) : base(sender)
    {
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllRanks.Query();

        var task = _sender.Send(query);

        return await FromResult(task);
    }
}