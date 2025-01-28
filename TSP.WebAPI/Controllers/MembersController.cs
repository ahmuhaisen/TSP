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

namespace TSP.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ApiController
{
    public MembersController(ISender sender) : base(sender)
    { }

    [HttpGet("CommitteeMembers")]
    [ProducesResponseType(typeof(List<MembersListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getCommitteeMembers([FromQuery] Guid societyId, [FromQuery] bool isCommittee)
    {
        var query = GetAllSocietyMembers.Query.Create(societyId, isCommittee);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpPut("AddCommittee")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> addCommittee(Guid StudentId, string Position, DateOnly StudentDate)
    {
        var query = AddCommittee.Command.Create(StudentId, Position, StudentDate);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

}
