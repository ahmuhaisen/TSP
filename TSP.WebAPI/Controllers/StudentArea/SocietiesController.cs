using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Students.Commands;
using TPS.Application.Areas.StudentArea.Societies.Commands;
using TPS.Application.Areas.StudentArea.Societies.Contracts;
using TPS.Application.Areas.StudentArea.Societies.Contracts.Requests;
using TPS.Application.Areas.StudentArea.Societies.Queries;
using TPS.Application.Areas.StudentArea.Socities.Commands;
using TPS.Application.Areas.StudentArea.Students.Contracts.Requests;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.StudentArea;

[ApiController]
[Route($"api/{Constants.APIAreas.Student}/[controller]")]
public class SocietiesController : ApiController
{
    public SocietiesController(ISender sender) : base(sender)
    { }

    [HttpGet("{societyId}")]
    [ProducesResponseType(typeof(SocietyDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getSocietyById(Guid SocietyId)
    {
        var query = GetSocietyById.Query.Create(SocietyId);
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    [HttpDelete("{societyId}/Members/{studentId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteMemberFromSociety(Guid StudentId, Guid SocietyId)
    {
        var query = LeaveSociety.Command.Create(StudentId, SocietyId);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpPost("{SocietyId}/Members/{StudentId}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostJoinSocietyRequest(JoinSocietyRequest request, Guid StudentId,Guid SocietyId)
    {
        var command = JoinSociety.Command.Create(request.StudentId,
                                               request.SocietyId,
                                               request.Motivation,
                                               request.Section);
        var task = _sender.Send(command);
        return await FromResult(task);
    }

    [HttpGet("{SocietyId}/Members/Requests")]
    [ProducesResponseType(typeof(List<MembershipRequestDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMembershipRequestsOfManagedSociety(Guid SocietyId)
    {
        var query = MembershipRequestsOfManagedSocieties.Query.Create(SocietyId, base.GetCurrentUserId()!.Value);
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    [HttpPut("{SocietyId}/Members/Requests/{MembershipRequestId}/{isAccepted}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMembershipRequestStatus([FromRoute] Guid MembershipRequestId,
                                                                      [FromQuery] bool isAccpeted,
                                                                      [FromRoute] Guid SocietyId,
                                                                      UpdateMembershipRequest request)
    {
        var query = MembershipRequestStatusUpdate.Command.Create(MembershipRequestId, SocietyId, isAccpeted, base.GetCurrentUserId()!.Value);
        var task = _sender.Send(query);
        return await FromResult(task);
    }
}