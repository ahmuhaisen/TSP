using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Advisors.Queries;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Societies.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.AdminArea;


[ApiController]
[Route($"api/{Constants.APIAreas.Admin}/[controller]")]
public class AdvisorController : ApiController
{
    public AdvisorController(ISender sender) : base(sender)
    { }

    [HttpGet("AdvisorSocieties")]
    [ProducesResponseType(typeof(List<SocietyListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getAdvisorSocieties([FromQuery] Guid advisorIds)
    {
        var query = GetAdvisorSocieties.Query.Create(advisorIds);

        var task = _sender.Send(query);

        return await FromResult(task);
    }
    [HttpGet("OtherSocieties")]
    [ProducesResponseType(typeof(List<SocietyListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getOtherSocieties([FromQuery] Guid advisorIds)
    {
        var query = GetOtherSocieties.Query.Create(advisorIds);

        var task = _sender.Send(query);

        return await FromResult(task);
    }
    //[Authorize(Roles = "Faculty")]
    [HttpGet("AllAdvisors")]
    [ProducesResponseType(typeof(List<SocietyListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getAllAdvisors()
    {
        var query = GetAllFacultyMembers.Query.Create();

        var task = _sender.Send(query);

        return await FromResult(task);
    }


}
