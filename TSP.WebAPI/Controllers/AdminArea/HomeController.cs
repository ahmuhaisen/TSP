using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Advisors.Queries;
using TPS.Application.Areas.AdminArea.Home.Contracts;
using TPS.Application.Areas.AdminArea.Home.Queries;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.AdminArea.Students.Queries;
using TPS.Application.Areas.Shared.Events.Contracts;
using TPS.Application.Areas.Shared.Events.Queries;
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
    [HttpGet("Advisors")]
    [ProducesResponseType(typeof(FacultyMemberBasicDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAdvisorByName([FromQuery] string? searchTerm)
    {
        var query = SearchAdvisor.Query.Create(searchTerm);

        var task = _sender.Send(query);
      
        return await FromResult(task);
    }

    [HttpGet("Students")]
    [ProducesResponseType(typeof(StudentBasicDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchStudentByName([FromQuery] string? searchTerm)
    {
        var query = SearchStudent.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpGet("Events")]
    [ProducesResponseType(typeof(EventBasicDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchEventByName([FromQuery] string? searchTerm)
    {
        var query = SearchEvent.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

}
