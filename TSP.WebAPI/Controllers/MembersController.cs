using MediatR;
using Microsoft.AspNetCore.Mvc;
using TSP.Domain.Shared;
using TPS.Application.Societies.Commands;
using TPS.Application.Societies.Contracts.Requests;
using TPS.Application.Societies.Contracts;
using TPS.Application.Societies.Queries;
using TPS.Application.Students.Contracts;

namespace TPS.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ApiController
{
    public MembersController(ISender sender) : base(sender)
    {}

    [HttpGet("/getCommitteeMembers")]
    [ProducesResponseType(typeof(List<MembersListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getCommitteeMembers([FromQuery] Guid societyId)
    {
        var query = GetCommitteeMembers.Query.Create(societyId);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

}
