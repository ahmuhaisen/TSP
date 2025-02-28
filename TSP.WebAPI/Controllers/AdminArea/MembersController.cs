using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Students.Commands;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.StudentArea.Students.Contracts.Requests;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.AdminArea;

[ApiController]
[Route($"api/{Constants.APIAreas.Admin}/[controller]")]
public class MembersController : ApiController
{
    public MembersController(ISender sender) : base(sender)
    { }

    [HttpGet("AllSocietyMembers")]
    [ProducesResponseType(typeof(List<MembersListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getCommitteeMembers([FromQuery] Guid societyId, [FromQuery] bool isCommittee)
    {
        var query = GetAllSocietyMembers.Query.Create(societyId, isCommittee);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpPut("Committee")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> addCommittee(AddCommitteeRequest request)
    {
        var query = AddCommittee.Command.Create(request);

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
