using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IDBManagerService
{
    public Task<string> UploadFileToGitHub(string filePath, string base64Content);

}

public interface IFileManagerService {
    public Task<string> SaveImage(string base64Image, string folder);
}
