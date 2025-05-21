using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Societies.Commands;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Societies.Contracts.Requests;
using TPS.Application.Areas.Shared.Abstractions;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.SuperAdminArea;

[ApiController]
[Authorize(Roles = "SuperAdmin")]
[Route($"api/{Constants.APIAreas.SuperAdmin}/[controller]")]
public class SocietiesController(ISender sender, ISocietiesService _societiesService) : ApiController(sender)
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SocietyDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll()
    {
        var task = _societiesService.getAllSocieties();
        return await FromResult(task);
    }


    [HttpGet("{societyId}")]
    [ProducesResponseType(typeof(SocietyDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid societyId)
    {
        var query = GetSocietyById.Query.Create(societyId);

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
                                                   request.LogoBase64,
                                                   request.CreationDate,
                                                   request.ThemeColor,
                                                   request.AdvisorId);

        var task = _sender.Send(command);

        return await FromResult(task);
    }

    [HttpPut("{societyId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(Guid societyId, UpdateSocietyRequest request)
    {
        var command = UpdateSociety.Command.Create(societyId,
                                                   request.Name,
                                                   request.Description,
                                                   request.LogoBase64,
                                                   request.ThemeColor,
                                                   request.CreationDate,
                                                   request.AdvisorId
                                                   );
        var task = _sender.Send(command);

        return await FromResult(task);
    }
}