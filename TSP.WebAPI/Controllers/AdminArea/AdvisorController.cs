using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Advisors.Queries;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Societies.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.AdminArea;


[ApiController]
[Route($"api/{Constants.APIAreas.Admin}/[controller]")]
public class FacultyMemberController : ApiController
{
    public FacultyMemberController(ISender sender) : base(sender)
    { }

    [HttpGet("Societies/Advised")]
    [ProducesResponseType(typeof(List<SocietyListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getAdvisorSocieties()
    {
        var query = GetAdvisorSocieties.Query.Create(GetCurrentUserId()!.Value);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpGet("Societies/Other")]
    [ProducesResponseType(typeof(List<SocietyListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getOtherSocieties()
    {
        var query = GetOtherSocieties.Query.Create(GetCurrentUserId()!.Value);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    // AdminArea/FacultyMembers
    [HttpGet]
    [ProducesResponseType(typeof(List<SocietyListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getAllAdvisors()
    {
        var query = GetAllFacultyMembers.Query.Create();

        var task = _sender.Send(query);

        return await FromResult(task);
    }
}
