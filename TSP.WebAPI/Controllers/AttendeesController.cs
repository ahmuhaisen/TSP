using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Attendees.Commands;
using TPS.Application.Attendees.Contracts;
using TPS.Application.Attendees.Contracts.Requests;
using TPS.Application.Attendees.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendeesController : ApiController
{
    public AttendeesController(ISender sender) : base(sender)
    {}

    [HttpGet]
    [ProducesResponseType<List<AttendeeBasicDetailsDTO>>(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAttendees(Guid eventId)
    {
        var query = new GetEventAttendees.Query(eventId);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAttendee(CreateEventAttendeeRequest request)
    {
        var command = new CreateEventAttendee.Command(
            request.FullName,
            request.Email,
            request.UniversityNumber,
            request.PhoneNumber,
            request.Notes,
            request.DepartmentId,
            request.EventId
        );

        var task = _sender.Send(command);

        return await FromResult(task);
    }
}
