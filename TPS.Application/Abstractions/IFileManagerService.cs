using TSP.Domain.Shared;

public interface IGitHubService
{
    public Task<Result<string>> uploadFile(string filePath, string base64Content);
    public Task<Result<string>> getFile(string path);
    public Task<Result<string>> deleteFile(string path);
    public Task<Result<string>> updateFile(string path, string base64Content);
    Task<Result<string>> uploadFile_EXPERIMENTAL(string filePath, string base64Content);
}

