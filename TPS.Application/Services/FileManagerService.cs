using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Options;
using static System.Net.WebRequestMethods;

namespace TPS.Application.Services;

public class GitHubService : IDBManagerService
{
    private readonly IOptions<GitOptions> options;
    public required string URL {  get; set; }
    public GitHubService(IOptions<GitOptions> options)
    {
        this.options = options;
        this.URL = $"https://api.github.com/repos/{options.Value.UserName}/{options.Value.Repo}/contents/";
    }
    private string generateCommitMessage(string message)
    {
        return message;
    }

    public async Task<string> UploadFileToGitHub(string filePath, string base64Content)
    {

        this.URL += filePath;

        var requestBody = new
        {
            message = this.generateCommitMessage(nameof(UploadFileToGitHub)+": "+filePath),
            content = base64Content
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        using (var httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.Value.Token);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "CSharp-App");

            var response = await httpClient.PutAsync(this.URL, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return string.Empty;
            }
            else
            {
                string error = await response.Content.ReadAsStringAsync();
                return $"GitHub API error: {response.StatusCode} - {error}";
                throw new Exception($"GitHub API error: {response.StatusCode} - {error}");
            }
        }
    }
}
public class FileManager: IFileManagerService
{
    public const string ServicesImagesPath = "/Images/Services";
    public const string MenusImagesPath = "/Images/Menus";
    public const string ItemsImagesPath = "/Images/Items";
    public const string OptionsImagesPath = "/Images/Options";
    public const string ValidImageFormats = "jpg,jpeg,png,webp";
    public const int MaxImageSizeInMB = 2;
    private readonly IDBManagerService _FileManagerService;
    public FileManager(IDBManagerService fileManagerService)
    {
        _FileManagerService = fileManagerService;

    }

    public async Task<string> SaveImage(string base64Image, string folder)
    {
        if (!IsValidBase64ImageString(base64Image))
        {
            return string.Empty;
        }
        if (!IsValidImageType(base64Image))
        {
            return string.Empty;
        }
        base64Image = base64Image.Split(',')[1];
        string imageId = Guid.NewGuid().ToString();
        string fileName = folder+"/"+imageId;
        string result =await _FileManagerService.UploadFileToGitHub(fileName, base64Image);
        if(!string.IsNullOrEmpty(result))
        {
            return string.Empty;
        }

        return imageId;
    }

    //public string SaveImage2(string base64Image, string directoryPath)
    //{
    //    string content = base64Image.Substring(base64Image.IndexOf(',') + 1);
    //    byte[] imageBytes = Convert.FromBase64String(content);

    //    string imageID = generateImageID(base64Image);

    //    string wwwRootPath = _pathProvider.GetWebRootPath();
    //    string folderPath = wwwRootPath + "/" + directoryPath;

    //    Directory.CreateDirectory(folderPath);

    //    string filePath = folderPath + "/" + imageID;

    //    File.WriteAllBytes(filePath, imageBytes);

    //    return imageID;
    //}

    //public string? UpdateImage(string? existingImageId, string? newBase64Image, string path)
    //{
    //    if (string.IsNullOrEmpty(newBase64Image) && !string.IsNullOrEmpty(existingImageId))
    //    {
    //        DeleteImage(existingImageId, path);
    //        return null;
    //    }

    //    if (!string.IsNullOrEmpty(newBase64Image))
    //    {
    //        if (!string.IsNullOrEmpty(existingImageId))
    //        {
    //            DeleteImage(existingImageId, path);
    //        }

    //        var newImageID = SaveImage2(newBase64Image, path);
    //        return newImageID;
    //    }

    //    return null;
    //}

    //public void DeleteImage(string imageId, string directoryPath)
    //{
    //    string wwwRootPath = _pathProvider.GetWebRootPath();
    //    string imagePath = wwwRootPath + directoryPath + "/" + imageId;

    //    if (File.Exists(imagePath))
    //        File.Delete(imagePath);
    //}

    private bool IsValidBase64ImageString(string base64String)
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

public class GitOptions
{
    public string Token { get; set; }
    public string UserName { get; set; }
    public string Repo { get; set; }
    
}
