using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.StudentArea.Events.Commands;
using TPS.Application.Areas.StudentArea.Events.Contracts;
using TPS.Application.Areas.StudentArea.Events.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.StudentArea;

[ApiController]
[Route($"api/{Constants.APIAreas.Student}/[controller]")]
public class EventsController : ApiController
{
    public EventsController(ISender sender) : base(sender)
    {

    }

    [HttpGet("Requests")]
    [ProducesResponseType(typeof(MemberEventDetailsDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllEvents()
    {
        var query = GetMemberEventsRequests.Query.Create(GetCurrentUserId()!.Value);
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    [HttpPost("Requests")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> createEventRequest(AddEventRequest request)
    {
        var query = CreateEventRequest.Command.Create(request);
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    [HttpGet]
    [ProducesResponseType(typeof(EventSimpleDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getAllEventsByMonth([FromQuery]string date)
    {
        var query = GetEventsByMonth.Query.Create(date);
        var task = _sender.Send(query); 
        return await FromResult(task);
    }


}

