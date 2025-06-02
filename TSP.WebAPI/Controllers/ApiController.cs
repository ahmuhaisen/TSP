using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TSP.Domain.Enums;
using TSP.Domain.Shared;

namespace TSP.WebAPI.Controllers;

public abstract class ApiController : ControllerBase
{
    protected readonly ISender _sender;

    protected ApiController(ISender sender)
    {
        _sender = sender;
    }

    [AllowAnonymous]
    [HttpGet("Ping")]
    public object Ping()
    {
        return Ok(true);
    }

    protected async Task<IActionResult> FromResult<TData>(Task<Result<TData>> task)
    {
        if (task is null)
            return BadRequest(ResponseEnvelope.Failure(Error.InternalServerError("The provided task is null.")));

        try
        {
            var result = await task;

            if (result.IsSuccess)
            {
                var envelope = ResponseEnvelope.Success(result.Data!);
                return Ok(envelope);
            }

            var errorEnvelope = ResponseEnvelope.Failure(result.Error);
            return result.Error.Code switch
            {
                ErrorCode.NotFound => NotFound(errorEnvelope),
                _ => BadRequest(errorEnvelope)
            };
        }
        catch (Exception ex)
        {
            var errorEnvelope = ResponseEnvelope.Failure(Error.InternalServerError(ex.Message));
            return BadRequest(errorEnvelope);
        }
    }

    protected async Task<IActionResult> FromResult(Task<Result> task)
    {
        if (task is null)
            return BadRequest(ResponseEnvelope.Failure(Error.InternalServerError("The provided task is null.")));

        try
        {
            var result = await task;

            if (result.IsSuccess)
            {
                var envelope = ResponseEnvelope.Success(true);
                return Ok(envelope);
            }

            var errorEnvelope = ResponseEnvelope.Failure(result.Error);
            return result.Error.Code switch
            {
                ErrorCode.NotFound => NotFound(errorEnvelope),
                _ => BadRequest(errorEnvelope)
            };
        }
        catch (Exception ex)
        {
            var errorEnvelope = ResponseEnvelope.Failure(Error.InternalServerError(ex.Message));
            return BadRequest(errorEnvelope);
        }
    }


    protected virtual Guid? GetCurrentUserId()
    {
        var userId = User.FindFirstValue("uid");

        if (string.IsNullOrEmpty(userId))
            return null;

        return Guid.Parse(userId);
    }

    protected virtual UserType GetCurrentUserType()
    {
        var userTypeClaim = User.FindFirstValue("rle");

        if (Enum.TryParse<UserType>(userTypeClaim, ignoreCase: true, out var userType))
            return userType;

        if (userTypeClaim == "Faculty")
            return UserType.FacultyMember;
        throw new UnauthorizedAccessException("Invalid or missing user type.");
    }
}
