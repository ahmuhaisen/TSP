using MediatR;
using Microsoft.AspNetCore.Mvc;
using TSP.Domain.Shared;
using TPS.Application.Societies.Commands;
using TPS.Application.Societies.Contracts.Requests;
using TPS.Application.Societies.Contracts;
using TPS.Application.Societies.Queries;
using TPS.Application.Students.Contracts;
using System.Runtime.InteropServices;
using TPS.Application.Students.Commands;

namespace TPS.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ApiController
{
    public MembersController(ISender sender) : base(sender)
    {}

    [HttpGet("GetAllSocietyMembers")]
    [ProducesResponseType(typeof(List<MembersListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getCommitteeMembers([FromQuery] Guid societyId,[FromQuery]bool isCommittee)
    {
        var query = GetAllSocietyMembers.Query.Create(societyId, isCommittee);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpPut("Committee")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> addCommittee(Guid StudentId,Guid SocietyId, string Position, DateOnly StudentDate)
    {
        var query = AddCommittee.Command.Create(StudentId, SocietyId, Position,StudentDate);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpDelete("Committee")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> deleteCommittee(Guid StudentId, Guid SocietyId)
    {
        var query = DeleteCommittee.Command.Create(StudentId, SocietyId);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

}
