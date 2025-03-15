using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.StudentArea.Membership.Commands;
using TPS.Application.Areas.StudentArea.Membership.Contracts;
using TPS.Application.Areas.StudentArea.Membership.Contracts.Requests;
using TPS.Application.Areas.StudentArea.Membership.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.StudentArea
{
    [ApiController]
    [Route($"api/{Constants.APIAreas.Student}/[controller]")]
    public class MembershipController(ISender sender) : ApiController(sender)
    {
        [HttpPost("{StudentId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult>PostJoinSocietyRequest(JoinSocietyRequest request,Guid StudentId)
        {
            var command = JoinSociety.Command.Create(request.StudentId,
                                                   request.SocietyName,
                                                   request.Motivation,
                                                   request.Section);
            var task = _sender.Send(command);
            return await FromResult(task);
        }

        [HttpGet("{SocietyId}")]
        [ProducesResponseType(typeof(List<MembershipRequestDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult>GetMembershipRequestsOfManagedSociety(Guid SocietyId)
        {
            var query = MembershipRequestsOfManagedSocieties.Query.Create(SocietyId, base.GetCurrentUserId()!.Value);
            var task = _sender.Send(query);
            return await FromResult(task);
        }
        [HttpPut("{MembershipRequestId}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult>UpdateMembershipRequestStatus([FromRoute]Guid MembershipRequestId,
                                                                      [FromQuery]bool isAccpeted,
                                                                      [FromRoute]Guid SocietyId)
        {
            var query = MembershipRequestStatusUpdate.Command.Create(MembershipRequestId, SocietyId,isAccpeted, base.GetCurrentUserId()!.Value);
            var task = _sender.Send(query);
            return await FromResult(task);
        }
        [HttpGet]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult>StudentMembershipRequests()
        {
            var query = MembershipRequestsOfStudent.Query.Create(base.GetCurrentUserId()!.Value);
            var task = _sender.Send(query);
            return await FromResult(task);
        }
    }
}
