using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TPS.Application.Abstractions;

namespace TSP.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NotificationsController(ISender sender, INotificationService _notificationService) : ApiController(sender)
{
    [HttpGet]
    public async Task<IActionResult> GetAllUserNotifications()
    {
        var userId = GetCurrentUserId();

        if (userId is null)
            return Unauthorized();

        var task = _notificationService.GetAllUserNotifications(userId.Value);

        return await FromResult(task);
    }


    [HttpPut("{notificationId}/mark-as-read")]
    public async Task<IActionResult> MarkNotificationAsRead(Guid notificationId)
    {
        var userId = GetCurrentUserId();

        if (userId is null)
            return Unauthorized();

        var task = _notificationService.MarkNotificationAsReadAsync(notificationId, userId.Value);

        return await FromResult(task);
    }

    [HttpPut("mark-all-as-read")]
    public async Task<IActionResult> MarkAllNotificationsAsRead()
    {
        var userId = GetCurrentUserId();

        if (userId is null)
            return Unauthorized();

        var task = _notificationService.MarkAllNotificationsAsReadAsync(userId.Value);

        return await FromResult(task);
    }
}
