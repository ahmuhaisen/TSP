using MediatR;
using Microsoft.AspNetCore.Mvc;
using TSP.Domain.Shared;
using TPS.Application.Societies.Commands;
using TPS.Application.Societies.Contracts.Requests;
using TPS.Application.Societies.Contracts;
using TPS.Application.Societies.Queries;
using TSP.Domain.Entities;

namespace TPS.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdvisorController : ApiController
{
    public AdvisorController(ISender sender) : base(sender)
    {}
    [HttpGet("/getAdvisorSocieties")]
    [ProducesResponseType(typeof(List<SocietyListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getAdvisorSocieties([FromQuery] Guid advisorIds)
    {
        var query = GetAdvisorSocieties.Query.Create(advisorIds);

        var task = _sender.Send(query);

        return await FromResult(task);
    }
    public async Task<IActionResult> getOtherSocieties([FromQuery] Guid advisorIds)
    {
        var query = GetOtherSocieties.Query.Create(advisorIds);

        var task = _sender.Send(query);

        return await FromResult(task);
    }


}
