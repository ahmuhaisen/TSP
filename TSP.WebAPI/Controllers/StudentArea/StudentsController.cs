using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.StudentArea.Societies.Queries;
using TPS.Application.Areas.StudentArea.Socities.Commands;
using TPS.Application.Areas.StudentArea.Students.Commands;
using TPS.Application.Areas.StudentArea.Students.Contracts.Requests;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers.StudentArea;

[ApiController]
[Route($"api/{Constants.APIAreas.Student}/[controller]")]
public class StudentsController : ApiController
{
    public StudentsController(ISender sender) : base(sender)
    { }
    [HttpGet("OtherSocieties")]
    public async Task<IActionResult> GetOtherSocieties()
    {
        var query = GetMemberOtherSocieties.Query.Create(GetCurrentUserId()!.Value);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpGet("AllSocieties")]
    public async Task<IActionResult> getMemberSocieties()
    {
        var query = GetMemberSocieties.Query.Create(GetCurrentUserId()!.Value);
        var task = _sender.Send(query);
        return await FromResult(task);
    }


}

