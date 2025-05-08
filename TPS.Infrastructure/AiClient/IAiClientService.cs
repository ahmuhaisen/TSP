namespace TPS.Infrastructure.AiClient;

public interface IAiClientService
{
    Task<string> GetResponseAsync(string prompt);
}
