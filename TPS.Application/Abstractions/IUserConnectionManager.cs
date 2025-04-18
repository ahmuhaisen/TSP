namespace TPS.Application.Abstractions;

public interface IUserConnectionManager
{
    void AddConnection(string userId, string connectionId);
    void RemoveConnection(string connectionId);
    List<string> GetConnections(string userId);
    List<string> GetOnlineUserIds();
}