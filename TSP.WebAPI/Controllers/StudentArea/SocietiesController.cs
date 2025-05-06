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

    [HttpDelete("{societyId}/Members")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteMemberFromSociety([FromRoute] Guid societyId)
    {
        var query = LeaveSociety.Command.Create(base.GetCurrentUserId()!.Value, societyId);

        var task = _sender.Send(query);

        return await FromResult(task);
    }
    [HttpDelete("{societyId}/Members/{memberId}/kick")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> KickMemberFromSociety([FromRoute] Guid societyId,
                                                            [FromRoute] Guid memberId)
    {
        var query = KickMember.Command.Create(societyId,
            memberId, base.GetCurrentUserId()!.Value);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpPost("{SocietyId}/JoinRequest")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostJoinSocietyRequest(Guid SocietyId, JoinSocietyRequest request)
    {


        var command = JoinSociety.Command.Create(request.StudentId,
                                               SocietyId,
                                               request.Motivation,
                                               request.Section);
        var task = _sender.Send(command);
        return await FromResult(task);
    }

    [HttpGet("{SocietyId}/Members/Requests")]
    [ProducesResponseType(typeof(List<MembershipRequestDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMembershipRequestsOfManagedSociety([FromRoute] Guid SocietyId)
    {
        var query = MembershipRequestsOfManagedSocieties.Query.Create(SocietyId, base.GetCurrentUserId()!.Value);
        var task = _sender.Send(query);
        return await FromResult(task);
    }

    [HttpPut("{SocietyId}/Members/Requests/{MembershipRequestId}/{isAccepted}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMembershipRequestStatus([FromRoute] Guid SocietyId,
                                                                [FromRoute] Guid MembershipRequestId,
                                                                      [FromRoute] bool isAccepted

                                                                      )
    {


        var query = MembershipRequestStatusUpdate.Command.Create(MembershipRequestId, SocietyId, isAccepted, base.GetCurrentUserId()!.Value);
        var task = _sender.Send(query);
        return await FromResult(task);
    }
}