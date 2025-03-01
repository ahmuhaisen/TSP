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
    public async Task<IActionResult> GetAll()
    {
        // This api returns all event requests for the current user
        // TODO: Rename the query to GetEventRequests
        // TODO: Rename the Dto to EventRequestDTO
        // If the current user is an Advisor => return all event requests that are not approved by the advisor
        // If the current user is a Dean or Dean Assistant => return all event requests that are approved by the advisor but not by the dean or dean assistant
        var query = GetAllEvents.Query.Create(GetCurrentUserId()!.Value);
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    [HttpGet("{eventId}")]
    [ProducesResponseType(typeof(List<EventDetailsDTO>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> EventDetails(Guid EventId)
    {
        var query = GetEventDetails.Query.Create(EventId);
        var task = _sender.Send(query);
        return await FromResult(task);
    }


    // TODO: 
    // AdminArea/Events/:eventId/Decision?isAccepted=true // Accept
    // AdminArea/Events/:eventId/Decision?isAccepted=false&remark=SomeRemark // Reject
    [HttpPut("{eventId}/Decision")]
    [ProducesResponseType(typeof(Guid),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Accept(Guid EventId, [FromQuery] bool isAccepted, [FromQuery] string? remark)
    {
        // TODO: Rename the command to AcceptEventCommand
        // If the current user is an Advisor => If the advisor decision is made, return 400 // The advisor can't change his decision
        // If the current user is a Dean or Dean Assistant => If the advisor decision is not made, return 400
        // If the current user is a Dean or Dean Assistant => If the advisor decision is made => 
        //     advisor decision is accepted => continue with the dean decision
        //     advisor decision is rejected => return 400
        var query= AcceptEvent.Command.Create(EventId,GetCurrentUserId()!.Value);
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    // TODO: AdminArea/Events/:id/Attendees
}

