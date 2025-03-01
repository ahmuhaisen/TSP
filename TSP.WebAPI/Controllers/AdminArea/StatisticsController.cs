using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Statistics.Contracts;
using TPS.Application.Areas.AdminArea.Statistics.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.AdminArea;

[ApiController]
[Route($"api/{Constants.APIAreas.Admin}/[controller]")]
public class StatisticsController : ApiController
{
    public StatisticsController(ISender sender) : base(sender)
    {
    }
    [HttpGet("TopSocietiesByMembers")] //L1
    [ProducesResponseType(typeof(List<SocietyMembersCountDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getTopSocietiesByMembers([FromQuery] int numberOfSocities)
    {
        var query = GetMemForEachSociety.Query.Create(numberOfSocities);
        var task = _sender.Send(query);
        return await FromResult(task);
    }
    [HttpGet("TopEventsByAttendence")] //L2
    [ProducesResponseType(typeof(List<EventAttendanceCountDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getTopEventsByAttendence([FromQuery] int numberOfEvents)
    {
        var query = GetattendanceForEachEvent.Query.Create(numberOfEvents);
        var task = _sender.Send(query);
        return await FromResult(task);
    }
    [HttpGet("TopSocities")] //R2
    [ProducesResponseType(typeof(List<SocietyDataDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getTopSocities([FromQuery] int numberOfSocities)
    {
        var query = GetTopSocities.Query.Create(numberOfSocities);
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    [HttpGet("EventsByMonth")] //R1
    [ProducesResponseType(typeof(List<SocietyDataDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getLastMonths([FromQuery] int numberOfMonths)
    {
        var query =GetEventsPerMonth.Query.Create(numberOfMonths);
        var task = _sender.Send(query);
        return await FromResult(task);
    }





}
