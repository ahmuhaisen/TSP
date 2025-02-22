using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.StudentArea.Societies.Queries;
using TPS.Application.Areas.StudentArea.Socities.Commands;
using TPS.Application.Areas.StudentArea.Students.Commands;
using TPS.Application.Areas.StudentArea.Students.Contracts.Requests;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.StudentArea;

[ApiController]
[Route($"api/{Constants.APIAreas.Student}/[controller]")]
public class SocietiesController : ApiController
{
    public SocietiesController(ISender sender) : base(sender)
    { }


    [HttpGet("OtherSocieties")]
    public async Task<IActionResult> GetOtherSocieties([FromQuery] Guid StudentId)
    {
        var query = GetMemberOtherSocieties.Query.Create(StudentId);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpGet("AllSocieties")]
    public async Task<IActionResult> getMemberSocieties([FromQuery]Guid StudentId)
    {
        var query = GetMemberSocieties.Query.Create(StudentId);
        var task = _sender.Send(query); 
        return await FromResult(task);
    }
    [HttpGet("Society")]
    public async Task<IActionResult> getSocietyById([FromQuery] Guid SocietyId)
    {
        var query = GetSocietyById.Query.Create(SocietyId);
        var task = _sender.Send(query);
        return await FromResult(task);
    }
    [HttpDelete("SocietyMember")]
    public async Task<IActionResult> DeleteMemberFromSociety(Guid StudentId, Guid SocietyId)
    {
        var query = LeaveSociety.Command.Create(StudentId, SocietyId);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpPut("Committee")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> addCommittee(AddCommitteeRequest request)
    {
        var query = AddCommitteeMember.Command.Create(request);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

}