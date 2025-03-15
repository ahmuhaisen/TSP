using MediatR;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Areas.Shared.Profiles.Queries;

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
}