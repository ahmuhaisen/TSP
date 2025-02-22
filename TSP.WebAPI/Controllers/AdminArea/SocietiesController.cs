using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Societies.Commands;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Societies.Contracts.Requests;
using TPS.Application.Areas.AdminArea.Societies.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.AdminArea;

[ApiController]
[Route($"api/{Constants.APIAreas.Admin}/[controller]")]
public class SocietiesController : ApiController
{
    public SocietiesController(ISender sender) : base(sender)
    { }

    
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

    [HttpPost("Society")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post(CreateSocietyRequest request)
    {
        var command = CreateSociety.Command.Create(request.Name,
                                                   request.Description,
                                                   request.LogoBase64,
                                                   request.CreationDate,
                                                   request.ThemeColor,
                                                   request.AdvisorId);

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

    [HttpPut("Society")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(UpdateSocietyRequest request)
    {
        var command = UpdateSociety.Command.Create(request.Name,
                                                   request.Description,
                                                   request.LogoBase64,
                                                   request.ThemeColor,
                                                   request.Id);
        var task = _sender.Send(command);

        return await FromResult(task);

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
