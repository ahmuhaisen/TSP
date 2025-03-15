using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.AdminArea.Societies.Commands;
using TPS.Application.Areas.AdminArea.Societies.Contracts;
using TPS.Application.Areas.AdminArea.Societies.Contracts.Requests;
using TPS.Application.Areas.AdminArea.Students.Commands;
using TPS.Application.Areas.AdminArea.Students.Contracts;
using TPS.Application.Areas.StudentArea.Students.Contracts.Requests;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.AdminArea;

[ApiController]
[Route($"api/{Constants.APIAreas.Admin}/[controller]")]
public class SocietiesController : ApiController
{
    public SocietiesController(ISender sender) : base(sender)
    { }


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
        var command = UpdateSociety.Command.Create(request.Name,
                                                   request.Description,
                                                   request.LogoBase64,
                                                   request.ThemeColor,
                                                   societyId);
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

    // Members

    [HttpGet("{societyId}/Members")]
    [ProducesResponseType(typeof(List<MembersListDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> getCommitteeMembers(Guid societyId, [FromQuery] bool isCommittee)
    {
        var query = GetAllSocietyMembers.Query.Create(societyId, isCommittee);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpPut("{societyId}/Members/{studentId}/Committee")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> addCommittee(Guid societyId, Guid studentId, AddCommitteeRequest request)
    {
        var query = AddCommittee.Command.Create(studentId, societyId, request);

        var task = _sender.Send(query);

        return await FromResult(task);
    }
    [HttpPut("{societyId}/Members/{studentId}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> editMember(Guid societyId, Guid studentId, string position)
    {
        var query = EditMember.Command.Create(studentId, societyId, position);
        var task = _sender.Send(query);
        return await FromResult(task);
    }


    [HttpDelete("{societyId}/Members/{studentId}/Committee")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> deleteCommittee(Guid societyId, Guid studentId)
    {
        var query = DeleteCommittee.Command.Create(studentId, societyId);

        var task = _sender.Send(query);

        return await FromResult(task);
    }


}
