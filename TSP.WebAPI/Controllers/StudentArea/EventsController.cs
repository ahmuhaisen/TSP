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

    //api/studentArea/events/events
    [HttpGet("Events")]
    [ProducesResponseType(typeof(MemberEventDetailsDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllEvents()
    {
        var query = GetMemberEvents.Query.Create(GetCurrentUserId()!.Value);
        var task = _sender.Send(query);
        return await FromResult(task);    
    }

    [HttpPut("Event")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> addEvent(AddEventRequest request)
    {
        var query = AddEvent.Command.Create(request);
        var task = _sender.Send(query);
        return await FromResult(task);
    }
}

