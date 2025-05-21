using Microsoft.Extensions.AI;

namespace TPS.Infrastructure.AiClient;

public class AiClientService(IChatClient _chatClient) : IAiClientService
{
    public async Task<string> GetResponseAsync(string prompt)
    {
        var message = new ChatMessage(ChatRole.User, prompt);

        var response = await _chatClient.GetResponseAsync(message);

        var responseText = response.Messages.FirstOrDefault()?.Text;

        if (string.IsNullOrEmpty(responseText))
            throw new Exception("No response received from AI client.");

        return responseText;
    }
}
