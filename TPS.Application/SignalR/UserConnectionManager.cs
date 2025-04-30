using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using TPS.Application.Abstractions;

namespace TPS.Application.SignalR;


public class UserConnectionManager(ILogger<UserConnectionManager> _logger) : IUserConnectionManager
{
    private static readonly ConcurrentDictionary<string, List<string>> _connections = new();

    public void AddConnection(string userId, string connectionId)
    {
        _logger.LogWarning($"Adding connection for {userId}, connection Id {connectionId}");
        _connections.AddOrUpdate(userId,
            _ => new List<string> { connectionId },
            (_, existing) =>
            {
                existing.Add(connectionId);
                return existing;
            });

        _logger.LogWarning($"Number of connections {_connections.Count}");

    }

    public void RemoveConnection(string connectionId)
    {
        foreach (var pair in _connections)
        {
            pair.Value.Remove(connectionId);
            if (!pair.Value.Any())
                _connections.TryRemove(pair.Key, out _);
        }
    }

    public List<string> GetConnections(string userId)
    {
        return _connections.TryGetValue(userId, out var connections) ? connections : [];
    }

    public List<string> GetOnlineUserIds() => _connections.Keys.ToList();
}
