using MediatR;
using Microsoft.AspNetCore.Mvc;
using TSP.Domain.Shared;
using TPS.Application.Societies.Commands;
using TPS.Application.Societies.Contracts.Requests;
using TPS.Application.Societies.Contracts;
using TPS.Application.Societies.Queries;

namespace TPS.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SocietiesController : ApiController
{
    public SocietiesController(ISender sender) : base(sender)
    {}


    [HttpGet]
    [ProducesResponseType(typeof(List<SocietyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromQuery] string? searchTerm)
    {
        var query = GetAllSocieties.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SocietyDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = GetSocietyById.Query.Create(id);
        var task = _sender.Send(query);
        return await FromResult(task);
    }


    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(CreateSocietyRequest request)
    {
        var command = CreateSociety.Command.Create(request.Name,
                                                   request.Description,
                                                   request.LogoID,
                                                   request.CreationDate,
                                                   request.ThemeColor);

        var task = _sender.Send(command);

        return await FromResult(task);
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = DeleteSociety.Command.Create(id);
        var task = _sender.Send(command);
        return await FromResult(task);
    }
}
