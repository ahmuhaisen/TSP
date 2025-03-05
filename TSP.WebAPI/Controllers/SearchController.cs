using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.Shared.Events.Contracts;
using TPS.Application.Areas.Shared.Events.Queries;
using TPS.Application.Areas.Shared.Societies;
using TPS.Application.Areas.Shared.Students;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers;

[ApiController]
[Route($"api/{Constants.APIAreas.Shared}/[controller]")]
public class SearchController(ISender sender) : ApiController(sender)
{


    [HttpGet("Students")]
    [ProducesResponseType(typeof(List<StudentBasicDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> searchStudentByName([FromQuery] string? searchTerm)
    {
        var query = SearchStudent.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }


    [HttpGet("Events")]
    [ProducesResponseType(typeof(List<EventBasicDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> searchEvent([FromQuery] string? searchTerm)
    {
        var query = SearchEvent.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }


    [HttpGet("Societies")]
    [ProducesResponseType(typeof(List<SocietyListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> searchSociety([FromQuery] string? searchTerm)
    {
        var query = SearchSocities.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }


}
