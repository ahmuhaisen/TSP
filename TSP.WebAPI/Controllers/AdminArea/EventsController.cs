using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Events.Contracts;
using TPS.Application.Areas.AdminArea.Events.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.AdminArea;

[ApiController]
[Route($"api/{Constants.APIAreas.Admin}/[controller]")]
public class EventsController : ApiController
{
    public EventsController(ISender sender): base(sender)
    {
        
    }

    [HttpGet("Events")]
    [ProducesResponseType(typeof(List<EventsDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Events()
    {
        var query = GetAllEvents.Query.Create();
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    [HttpGet("eventDetails")]
    [ProducesResponseType(typeof(List<EventDetailsDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EventDetails([FromQuery] Guid EventId)
    {
        var query = GetEventDetails.Query.Create(EventId);
        var task = _sender.Send(query);
        return await FromResult(task);
    }
}

