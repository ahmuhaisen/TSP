using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.Shared.Profiles.Command;
using TPS.Application.Areas.Shared.Profiles.Contracts.Requests;
using TPS.Application.Areas.Shared.Profiles.Queries;
using TSP.Domain.Enums;
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
    public async Task<IActionResult> Get(Guid userId, [FromQuery] UserType userType)
    {
        var query = new GetUserProfile.Query(userId, userType);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [Authorize]
    [HttpGet("has-profile-image")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<IActionResult> HasProfileImage()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();

        var query = new HasProfileImage.Query(userId.Value);
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
                                                   request.FirstName!,
                                                   request.LastName!,
                                                   request.ProfileImageId!,
                                                   request.Email!,
                                                   request.Number!,
                                                   userType);
        var task = _sender.Send(command);
        return await FromResult(task);
    }


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        var query = new GetCurrentUserInfo.Query(GetCurrentUserId()!.Value, GetCurrentUserType()!);

        var task = _sender.Send(query);

        return await FromResult(task);
    }

    [HttpPut("reset/{userId}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseEnvelope), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> updatePassword([FromRoute]Guid userId,[FromQuery] string password)
    {


       var authHeader=  Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return Unauthorized("Missing or invalid Authorization header.");
        }
        var token = authHeader.Substring("Bearer ".Length).Trim();

        var command = UpdatePassword.Command.Create(
            userId,
            password, 
            token);

        var task = _sender.Send(command);
        return await FromResult(task);
    }

}