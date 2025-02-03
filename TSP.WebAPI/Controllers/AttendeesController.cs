using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.Attendees.Commands;
using TPS.Application.Areas.Attendees.Contracts;
using TPS.Application.Areas.Attendees.Contracts.Requests;
using TPS.Application.Areas.Attendees.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers;

[ApiController]
[Authorize]
[Route($"api/[controller]")]
public class AttendeesController : ApiController
{
    public AttendeesController(ISender sender) : base(sender)
    { }

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
    [AllowAnonymous]
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
