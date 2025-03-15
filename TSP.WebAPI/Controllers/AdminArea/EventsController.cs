using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Events.Commands;
using TPS.Application.Areas.AdminArea.Events.Contracts;
using TPS.Application.Areas.AdminArea.Events.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.AdminArea;

//[Authorize]
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
        // TODO: Rename the query to GetEventRequests DONE
        // TODO: Rename the Dto to EventRequestDTO DONE
        // If the current user is an Advisor => return all event requests that are not approved by the advisor
        // If the current user is a Dean or Dean Assistant => return all event requests that are approved by the advisor but not by the dean or dean assistant
        var query = EventRequest.Query.Create(Guid.Parse("91A9273B-098E-4A90-7B57-08DD5EF9673F"));
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


    // TODO: DONE
    // AdminArea/Events/:eventId/Decision?isAccepted=true // Accept
    // AdminArea/Events/:eventId/Decision?isAccepted=false&remark=SomeRemark // Reject
    [HttpPut("{eventRequestId}/Decision")]
    [ProducesResponseType(typeof(Guid),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateEventStatus([FromRoute]Guid EventRequestId, [FromQuery] bool isAccepted, [FromQuery] string? Remark)
    {
        // TODO: Rename the command to AcceptEventCommand ==> EventStatus DONE
        // If the current user is an Advisor => If the advisor decision is made, return 400 // The advisor can't change his decision
        // If the current user is a Dean or Dean Assistant => If the advisor decision is not made, return 400
        // If the current user is a Dean or Dean Assistant => If the advisor decision is made => 
        //     advisor decision is accepted => continue with the dean decision
        //     advisor decision is rejected => return 400
        //DONE
        var query= EventStatusUpdate.Command.Create(EventRequestId, base.GetCurrentUserId()!.Value,isAccepted,Remark);
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    // TODO: remove this api 
    // TODO: AdminArea/Events/:id/Attendees DONE
    [HttpGet("{eventRequestId}/Attendees")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AllEventAttendees([FromRoute] Guid EventRequestId)
    {
        var query = EventAttendeeInfo.Query.Create(EventRequestId);
        var task = _sender.Send(query);
        return await FromResult(task);
    }
}

