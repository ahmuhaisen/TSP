using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.Shared.Profiles.Command;
using TPS.Application.Areas.Shared.Profiles.Contracts.Requests;
using TPS.Application.Areas.Shared.Profiles.Queries;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfilesController : ApiController
{
    public ProfilesController(ISender sender) : base(sender)
    {
    }


    [HttpGet("{userId}")]
    public async Task<IActionResult> Get(Guid userId, [FromQuery] string userType)
    {
        var query = new GetUserProfile.Query(userId, userType);

        var task = _sender.Send(query);

        return await FromResult(task);
    }
    
    [Authorize]
    [HttpPut]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put([FromQuery] string userType, UpdateProfileRequest request)
    {
        var command = UpdateProfile.Command.Create(base.GetCurrentUserId()!.Value,
                                                   request.FullName,
                                                   request.ProfileImageId,
                                                   request.Email,
                                                   request.Number,
                                                   userType);
        var task = _sender.Send(command);
        return await FromResult(task);
    }
}