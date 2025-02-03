using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Home.Contracts;
using TPS.Application.Areas.AdminArea.Home.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.AdminArea;

[ApiController]
[Route($"api/{Constants.APIAreas.Admin}/[controller]")]
public class HomeController : ApiController
{
    public HomeController(ISender sender) : base(sender)
    {
    }
    //api/home
    [HttpGet("recentEvents")]
    [ProducesResponseType(typeof(List<EventListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Events([FromQuery] Guid advisorId)
    {
        var query = GetHomeEvents.Query.Create(advisorId);
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    [HttpGet("recentlyJoinedMembers")]
    [ProducesResponseType(typeof(List<RecentlyJoinedDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RecentlyJoinedMembers([FromQuery] string? searchTerm)
    {
        var query = GetRecentlyJoined.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpGet("homeStatistics")]
    [ProducesResponseType(typeof(HomeStatisticsDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HomeStatistics([FromQuery] string? searchTerm)
    {
        var query = GetHomeStatistics.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }
}
