using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Events.Commands;
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

    [HttpGet]
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
    [HttpPut("accept")]
    [ProducesResponseType(typeof(Guid),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Accept([FromQuery] Guid EventId)
    {
        var query= AcceptEvent.Command.Create(EventId,GetCurrentUserId()!.Value);
        var task = _sender.Send(query);
        return await FromResult(task);
    }
    [HttpPut("reject")]
    [ProducesResponseType(typeof(Guid),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Reject([FromQuery] Guid EventId, [FromQuery]string Remark)
    {
        var query= RejectEvent.Command.Create(EventId,GetCurrentUserId()!.Value,Remark);
        var task = _sender.Send(query);
        return await FromResult(task);
    }
}

