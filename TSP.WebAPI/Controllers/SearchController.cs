using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Advisors.Queries;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Societies.Queries;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.AdminArea.Students.Queries;
using TPS.Application.Areas.Shared.Events.Contracts;
using TPS.Application.Areas.Shared.Events.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers;

[ApiController]
[Route($"api/{Constants.APIAreas.Shared}/[controller]")]
public class SearchController(ISender sender) : ApiController(sender)
{
    [HttpGet("FacultyMembers")]
    [ProducesResponseType(typeof(FacultyMemberBasicDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchAdvisorByName([FromQuery] string? searchTerm)
    {
        var query = SearchFacultyMember.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpGet("Students")]
    [ProducesResponseType(typeof(StudentBasicDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchStudentByName([FromQuery] string? searchTerm)
    {
        var query = SearchStudent.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }


    [HttpGet("Events")]
    [ProducesResponseType(typeof(EventBasicDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchEventByName([FromQuery] string? searchTerm)
    {
        var query = SearchEvent.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }


    [HttpGet("Societies")]
    [ProducesResponseType(typeof(List<SocietyListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getSociety([FromQuery] string? searchTerm)
    {
        var query = GetAllSocieties.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }


}
