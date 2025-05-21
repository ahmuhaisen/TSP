using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TSP.Domain.Shared;
using TSP.Domain.Shared.Options;

namespace TPS.Application.Services;

public class GitHubService : IGitHubService
{
    private readonly IOptions<GitOptions> _options;
    private readonly HttpClient _httpClient;
    private const string _validImageFormats = "jpg,jpeg,png,webp";

    public required string BaseURL { get; set; }

    public GitHubService(HttpClient httpClient, IOptions<GitOptions> options)
    {
        _options = options;
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Value.Token);
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CSharp-App");
        BaseURL = $"https://api.github.com/repos/{options.Value.UserName}/{options.Value.Repo}/contents/";
    }

    [Obsolete]
    public async Task<Result<string>> uploadFile(string filePath, string base64Content)
    {
        if (!IsValidBase64ImageString(base64Content))
        {
            return Result.Failure<string>(Error.ValueInvalid("Not valid Base64 image"));
        }
        if (!IsValidImageType(base64Content))
        {
            return Result.Failure<string>(Error.ValueInvalid("Not valid image type"));
        }
        string imageId = generateImageId(base64Content);
        base64Content = base64Content.Split(',')[1];

        string fileName = filePath + "/" + imageId;

        string currentUrl = BaseURL + fileName;
        Console.WriteLine(currentUrl);
        var requestBody = new
        {
            message = $"Upload file: {fileName}",
            content = base64Content
        };

        var jsonContent = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8, "application/json");


        var response = await _httpClient.PutAsync(currentUrl, jsonContent);

        if (response.IsSuccessStatusCode)
        {
            return Result.Success(imageId);
        }
        else
        {
            string error = await response.Content.ReadAsStringAsync();
            return Result.Failure<string>(Error.InternalServerError(error));
        }

    }
    public async Task<Result<string>> getFile(string path)
    {
        string targetUrl = BaseURL + path;
        var response = await _httpClient.GetAsync(targetUrl);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            return Result.Failure<string>(Error.InternalServerError(""));
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        using var jsonDoc = JsonDocument.Parse(jsonResponse);
        var base64Content = jsonDoc.RootElement.GetProperty("content").GetString() ?? string.Empty;
        return Result.Success(base64Content);

    }
    public async Task<Result<string>> deleteFile(string filePath)
    {
        string sha = await getSha(filePath);
        string targetPath = BaseURL + filePath;

        if (string.IsNullOrEmpty(sha))
        {
            return Result.Failure<string>(Error.ValueInvalid("SHA does not exist"));
        }

        var requestBody = new
        {
            message = $"Delete file: {filePath}",
            sha
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Delete, targetPath)
        {
            Content = jsonContent
        };

        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return Result.Success($"{filePath}");
        }
        else
        {
            string error = await response.Content.ReadAsStringAsync();
            return Result.Failure<string>(Error.InternalServerError($"{sha}, {targetPath}"));

        }
    }
    public async Task<Result<string>> updateFile(string path, string base64Content)
    {
        string[]args = path.Split('/');
        Console.WriteLine("TESTING PATH " + path);
        Console.WriteLine("TESTING PATH " + path);
        Console.WriteLine("TESTING PATH " + path);

        if (args[1].IsNullOrEmpty())
        {
            Console.WriteLine("TESTING PATH " + path);
            return await uploadFile(args[0], base64Content);
        }


        if (!IsValidBase64ImageString(base64Content))
        {
            return Result.Failure<string>(Error.ValueInvalid("Not valid Base64 image"));
        }
        if (!IsValidImageType(base64Content))
        {
            return Result.Failure<string>(Error.ValueInvalid("Not valid image type"));
        }


        string currentUrl = BaseURL + path;
       


        var requestBody = new
        {
            message = $"Upload file: {path}",
            content = base64Content,
            sha = getSha(path)
        };

        var jsonContent = new StringContent(
           JsonSerializer.Serialize(requestBody),
           Encoding.UTF8, "application/json");


        var result = await _httpClient.PutAsync(currentUrl, jsonContent);

        if (result.IsSuccessStatusCode)
        {
            return Result.Success("");
        }
        else
        {

            string error = await result.Content.ReadAsStringAsync();
            return Result.Failure<string>(Error.InternalServerError(error));
        }
    }


    #region FileManagerHelpers
    private async Task<string> getSha(string filePath)
    {
        string targetPath = BaseURL + filePath;
        string getUrl = $"{BaseURL}/{filePath}";
        var response = await _httpClient.GetAsync(getUrl);

        if (!response.IsSuccessStatusCode)
            return string.Empty;
       

        var jsonResponse = await response.Content.ReadAsStringAsync();
  

        using var doc = JsonDocument.Parse(jsonResponse);

        Console.WriteLine(doc.RootElement.GetRawText());
     
        return doc.RootElement.GetProperty("sha").GetString() ?? string.Empty;

    }

    private bool IsValidBase64ImageString(string base64String)
    {
        if (!base64String.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            return false;

        int separatorIndex = base64String.IndexOf(';');
        if (separatorIndex < 0 || !base64String.Substring(5, separatorIndex - 5).StartsWith("image/"))
            return false;

        return true;
    }

    private static bool IsValidImageType(string base64Image)
    {
        string imageType = getImageType(base64Image);

        if (!_validImageFormats.Split(',').Contains(imageType))
            return false;

        return true;
    }

    private static string getImageType(string base64Image)
    {
        int separatorIndex = base64Image.IndexOf(';');
        return base64Image.Substring(5, separatorIndex - 5).Split('/')[1];
    }
    private string generateImageId(string Base64Image)
    {
        string s = getImageType(Base64Image);

        return Guid.NewGuid().ToString() + "." + s;
    }

    #endregion
}

