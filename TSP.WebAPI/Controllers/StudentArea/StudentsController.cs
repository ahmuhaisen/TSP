using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.Shared.Abstractions;
using TPS.Application.Areas.StudentArea.Societies.Queries;
using TPS.Application.Areas.StudentArea.Students.Queries;
using TSP.Domain.Shared;
namespace TSP.WebAPI.Controllers.StudentArea;

[ApiController]
[Route($"api/{Constants.APIAreas.Student}/[controller]")]
public class StudentsController(ISender sender, IStudentsService studentsService)  : ApiController(sender)
{
    [HttpGet("OtherSocieties")]
    public async Task<IActionResult> GetOtherSocieties()
    {
        var query = GetMemberOtherSocieties.Query.Create(GetCurrentUserId()!.Value);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpGet("AllSocieties")]
    public async Task<IActionResult> getMemberSocieties()
    {
        var query = GetMemberSocieties.Query.Create(GetCurrentUserId()!.Value,false);
        var task = _sender.Send(query);
        return await FromResult(task);
    }
    [HttpGet("AllCommitteeSocieties")]
    public async Task<IActionResult> getCommitteeSocities()
    {
        var query = GetMemberSocieties.Query.Create(GetCurrentUserId()!.Value,true);
        var task = _sender.Send(query);
        return await FromResult(task);
    }



    [HttpGet("MembershipRequests")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> StudentMembershipRequests()
    {
        var query = MembershipRequestsOfStudent.Query.Create(base.GetCurrentUserId()!.Value);
        var task = _sender.Send(query);
        return await FromResult(task);
    }


    [HttpGet("{studentId}/isCommittee")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> IsStudentACommitteeMember(Guid studentId)
    {
        var task = studentsService.IsStudentACommitteeMemberAsync(studentId);
        return await FromResult(task);
    }
}

