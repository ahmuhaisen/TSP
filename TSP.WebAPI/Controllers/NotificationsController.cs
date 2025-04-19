using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Abstractions;

namespace TSP.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController(ISender sender, INotificationService _notificationService) : ApiController(sender)
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllUserNotifications()
    {
        var userId = GetCurrentUserId();

        if (userId is null)
            return Unauthorized();

        var task = _notificationService.GetAllUserNotifications(userId.Value);

        return await FromResult(task);
    }
}
