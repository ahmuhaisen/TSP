using MediatR;
using Microsoft.AspNetCore.Mvc;
using TSP.Domain.Shared;
using TPS.Application.Societies.Commands;
using TPS.Application.Societies.Contracts.Requests;
using TPS.Application.Societies.Contracts;
using TPS.Application.Societies.Queries;

namespace TPS.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SocietiesController : ApiController
{
    public SocietiesController(ISender sender) : base(sender)
    {}

    [HttpGet]
    [ProducesResponseType(typeof(List<SocietyListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromQuery] string? searchTerm)
    {
        var query = GetAllSocieties.Query.Create(searchTerm);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpGet("{societyId}")]
    [ProducesResponseType(typeof(SocietyListDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid societyId)
    {
        var query = GetSocietyById.Query.Create(societyId);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpGet("{societyId}/members")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public Task<IActionResult> GetSocietyMembers(Guid societyId)
    {
        return Task.FromResult<IActionResult>(Ok("Not Implemented 2"));
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(CreateSocietyRequest request)
    {
        var command = CreateSociety.Command.Create(request.Name,
                                                   request.Description,
                                                   request.LogoId,
                                                   request.CreationDate,
                                                   request.ThemeColor);

        var task = _sender.Send(command);

        return await FromResult(task);
    }

    [HttpPost("{societyId}/members")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public Task<IActionResult> PostMember(Guid societyId)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{societyId}/advisor")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public Task<IActionResult> PostAdvisor(Guid societyId)
    {
        throw new NotImplementedException();
    }

    [HttpPut("{societyId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public Task<IActionResult> Put(Guid societyId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{societyId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid societyId)
    {
        var command = DeleteSociety.Command.Create(societyId);
        var task = _sender.Send(command);
        return await FromResult(task);
    }
}
