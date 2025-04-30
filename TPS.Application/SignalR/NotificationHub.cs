using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using TPS.Application.Abstractions;

namespace TPS.Application.SignalR;


public class NotificationHub(ILogger<NotificationHub> _logger, IUserConnectionManager _connectionManager) : Hub
{
    public override Task OnConnectedAsync()
    {
        _logger.LogInformation($"OnConnectedAsync...");

        var userId = Context.User?.FindFirst("uid")?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            _logger.LogInformation($"A user with id {userId} connected to the hub");
            _connectionManager.AddConnection(userId, Context.ConnectionId);
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation($"OnDisconnectedAsync...");
        
        _connectionManager.RemoveConnection(Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }
}
