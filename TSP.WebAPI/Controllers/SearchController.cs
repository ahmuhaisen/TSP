using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Societies.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers;

public class SearchController : ApiController
{
    public SearchController(ISender sender) : base(sender)
    {
    }
    [HttpGet("SocietySearch")]
    [ProducesResponseType(typeof(List<SocietyListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getSociety([FromQuery] string? searchTerm)
    {
        var query = GetAllSocieties.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }


}
