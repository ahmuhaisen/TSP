using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class FileManager
{
    public const string ServicesImagesPath = "Images/Services";
    public const string MenusImagesPath = "Images/Menus";
    public const string ItemsImagesPath = "Images/Items";
    public const string OptionsImagesPath = "Images/Options";
    public const string ValidImageFormats = "jpg,jpeg,png,webp";
    public const int MaxImageSizeInMB = 2;

    private const string GitHubRepoOwner = "your-github-username";
    private const string GitHubRepoName = "your-repo-name";
    private const string GitHubBranch = "main";
    private const string GitHubToken = "your-personal-access-token";

    private readonly HttpClient _httpClient;

    public FileManager()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("FileManagerApp");
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GitHubToken);
    }

    public async Task<string> SaveImage(string base64Image, string directoryPath)
    {
        // Decode the base64 image
        byte[] imageBytes = Convert.FromBase64String(base64Image);
        string fileName = Guid.NewGuid().ToString() + ".png";
        string filePath = $"{directoryPath}/{fileName}";

        // Convert image bytes to base64 for GitHub API
        string base64Content = Convert.ToBase64String(imageBytes);

        // Create the GitHub API request body
        var requestBody = new
        {
            message = $"Add image {fileName}",
            content = base64Content,
            branch = GitHubBranch
        };

        // Serialize the request body using System.Text.Json
        string jsonRequestBody = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

        // Send the request to GitHub API
        string apiUrl = $"https://api.github.com/repos/{GitHubRepoOwner}/{GitHubRepoName}/contents/{filePath}";
        var response = await _httpClient.PutAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            // Parse the response to get the file URL
            var responseContent = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseContent);
            JsonElement root = doc.RootElement;
            string downloadUrl = root.GetProperty("content").GetProperty("download_url").GetString();
            return downloadUrl;
        }
        else
        {
            throw new Exception($"Failed to upload image to GitHub: {response.ReasonPhrase}");
        }
    }

    public async Task<string> SaveImage2(string base64Image, string directoryPath)
    {
        // Extract the base64 content (remove the data URL prefix)
        string content = base64Image.Substring(base64Image.IndexOf(',') + 1);
        byte[] imageBytes = Convert.FromBase64String(content);

        // Generate a unique image ID
        string imageID = generateImageID(base64Image);
        string filePath = $"{directoryPath}/{imageID}";

        // Convert image bytes to base64 for GitHub API
        string base64Content = Convert.ToBase64String(imageBytes);

        // Create the GitHub API request body
        var requestBody = new
        {
            message = $"Add image {imageID}",
            content = base64Content,
            branch = GitHubBranch
        };

        // Serialize the request body using System.Text.Json
        string jsonRequestBody = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

        // Send the request to GitHub API
        string apiUrl = $"https://api.github.com/repos/{GitHubRepoOwner}/{GitHubRepoName}/contents/{filePath}";
        var response = await _httpClient.PutAsync(apiUrl, content);

        if (response.IsSuccessStatusCode)
        {
            // Parse the response to get the file URL
            var responseContent = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseContent);
            JsonElement root = doc.RootElement;
            string downloadUrl = root.GetProperty("content").GetProperty("download_url").GetString();
            return downloadUrl;
        }
        else
        {
            throw new Exception($"Failed to upload image to GitHub: {response.ReasonPhrase}");
        }
    }

    public async Task<string?> UpdateImage(string? existingImageId, string? newBase64Image, string path)
    {
        if (string.IsNullOrEmpty(newBase64Image) && !string.IsNullOrEmpty(existingImageId))
        {
            await DeleteImage(existingImageId, path);
            return null;
        }

        if (!string.IsNullOrEmpty(newBase64Image))
        {
            if (!string.IsNullOrEmpty(existingImageId))
            {
                await DeleteImage(existingImageId, path);
            }

            var newImageID = await SaveImage2(newBase64Image, path);
            return newImageID;
        }

        return null;
    }

    public async Task DeleteImage(string imageId, string directoryPath)
    {
        string filePath = $"{directoryPath}/{imageId}";

        // Get the current file SHA (required for deletion)
        string apiUrl = $"https://api.github.com/repos/{GitHubRepoOwner}/{GitHubRepoName}/contents/{filePath}";
        var response = await _httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseContent);
            JsonElement root = doc.RootElement;
            string sha = root.GetProperty("sha").GetString();

            // Create the GitHub API request body for deletion
            var requestBody = new
            {
                message = $"Delete image {imageId}",
                sha = sha,
                branch = GitHubBranch
            };

            // Serialize the request body using System.Text.Json
            string jsonRequestBody = JsonSerializer.Serialize(requestBody);
            var deleteContent = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json");

            // Send the delete request to GitHub API
            var deleteResponse = await _httpClient.DeleteAsync(apiUrl, deleteContent);

            if (!deleteResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to delete image from GitHub: {deleteResponse.ReasonPhrase}");
            }
        }
        else
        {
            throw new Exception($"Failed to fetch file SHA for deletion: {response.ReasonPhrase}");
        }
    }

    public static bool IsValidBase64ImageString(string base64String)
    {
        if (!base64String.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            return false;

        int separatorIndex = base64String.IndexOf(';');
        if (separatorIndex < 0 || !base64String.Substring(5, separatorIndex - 5).StartsWith("image/"))
            return false;

        return true;
    }

    public static bool IsValidImageType(string base64Image)
    {
        string imageType = getImageType(base64Image);

        if (!ValidImageFormats.Split(',').Contains(imageType))
            return false;

        return true;
    }

    private static string getImageType(string base64Image)
    {
        int separatorIndex = base64Image.IndexOf(';');
        return base64Image.Substring(5, separatorIndex - 5).Split('/')[1];
    }

    private static string generateImageID(string base64Image)
    {
        string name = Guid.NewGuid().ToString();
        string type = getImageType(base64Image);

        return $"{name}.{type}";
    }
}