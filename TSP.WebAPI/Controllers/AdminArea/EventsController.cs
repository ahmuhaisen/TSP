using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Events.Commands;
using TPS.Application.Areas.AdminArea.Events.Contracts;
using TPS.Application.Areas.AdminArea.Events.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.AdminArea;

[Authorize]
[ApiController]
[Route($"api/{Constants.APIAreas.Admin}/[controller]")]
public class EventsController : ApiController
{
    public EventsController(ISender sender): base(sender)
    {}

    [HttpGet]
    [ProducesResponseType(typeof(List<EventDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEventRequests()
    {
        // This api returns all event requests for the current user
        var query = EventRequest.Query.Create(GetCurrentUserId()!.Value);
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    [HttpGet("{eventRequestId}")]
    [ProducesResponseType(typeof(List<EventDetailsDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetEventDetails([FromRoute]Guid EventRequestId)
    {
        var query = EventDetails.Query.Create(EventRequestId);
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    [HttpPut("{eventRequestId}/Decision")]
    [ProducesResponseType(typeof(Guid),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEventStatus([FromRoute]Guid EventRequestId, [FromQuery] bool isAccepted, [FromQuery] string? Remark)
    {
        var query= EventStatusUpdate.Command.Create(EventRequestId, base.GetCurrentUserId()!.Value,isAccepted,Remark);
        var task = _sender.Send(query);
        return await FromResult(task);
    }
    }
    }
}

