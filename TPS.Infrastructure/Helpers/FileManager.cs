
//public class FileManager
//{
//    public const string ServicesImagesPath = "/Images/Services";
//    public const string MenusImagesPath = "/Images/Menus";
//    public const string ItemsImagesPath = "/Images/Items";
//    public const string OptionsImagesPath = "/Images/Options";
//    public const string ValidImageFormats = "jpg,jpeg,png,webp";
//    public const int MaxImageSizeInMB = 2;

//    private readonly IPathProvider _pathProvider;

//    public FileManager(IPathProvider pathProvider)
//    {
//        _pathProvider = pathProvider;
//    }

//    public string SaveImage(string base64Image, string directoryPath)
//    {
//        byte[] imageBytes = Convert.FromBase64String(base64Image);

//        string fileName = Guid.NewGuid().ToString() + ".png";

//        string wwwRootPath = _pathProvider.GetWebRootPath();
//        string folderPath = wwwRootPath + "/" + directoryPath;

//        Directory.CreateDirectory(folderPath);

//        string filePath = folderPath + "/" + fileName;

//        File.WriteAllBytes(filePath, imageBytes);

//        return fileName;
//    }

//    public string SaveImage2(string base64Image, string directoryPath)
//    {
//        string content = base64Image.Substring(base64Image.IndexOf(',') + 1);
//        byte[] imageBytes = Convert.FromBase64String(content);

//        string imageID = generateImageID(base64Image);

//        string wwwRootPath = _pathProvider.GetWebRootPath();
//        string folderPath = wwwRootPath + "/" + directoryPath;

//        Directory.CreateDirectory(folderPath);

//        string filePath = folderPath + "/" + imageID;

//        File.WriteAllBytes(filePath, imageBytes);

//        return imageID;
//    }

//    public string? UpdateImage(string? existingImageId, string? newBase64Image, string path)
//    {
//        if (string.IsNullOrEmpty(newBase64Image) && !string.IsNullOrEmpty(existingImageId))
//        {
//            DeleteImage(existingImageId, path);
//            return null;
//        }

//        if (!string.IsNullOrEmpty(newBase64Image))
//        {
//            if (!string.IsNullOrEmpty(existingImageId))
//            {
//                DeleteImage(existingImageId, path);
//            }

//            var newImageID = SaveImage2(newBase64Image, path);
//            return newImageID;
//        }

//        return null;
//    }

//    public void DeleteImage(string imageId, string directoryPath)
//    {
//        string wwwRootPath = _pathProvider.GetWebRootPath();
//        string imagePath = wwwRootPath + directoryPath + "/" + imageId;

//        if (File.Exists(imagePath))
//            File.Delete(imagePath);
//    }

//    public static bool IsValidBase64ImageString(string base64String)
//    {
//        if (!base64String.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
//            return false;

//        int separatorIndex = base64String.IndexOf(';');
//        if (separatorIndex < 0 || !base64String.Substring(5, separatorIndex - 5).StartsWith("image/"))
//            return false;

//        return true;
//    }

//    public static bool IsValidImageType(string base64Image)
//    {
//        string imageType = getImageType(base64Image);

//        if (!ValidImageFormats.Split(',').Contains(imageType))
//            return false;

//        return true;
//    }

//    private static string getImageType(string base64Image)
//    {
//        int separatorIndex = base64Image.IndexOf(';');
//        return base64Image.Substring(5, separatorIndex - 5).Split('/')[1];
//    }

//    private static string generateImageID(string base64Image)
//    {
//        string name = Guid.NewGuid().ToString();
//        string type = getImageType(base64Image);

//        return $"{name}.{type}";
//    }
//}