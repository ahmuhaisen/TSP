using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSP.Domain.Shared;

public interface IFileManagerService
{
    public Task<Result<string>> uploadFile(string filePath, string base64Content);
    public Task<Result<string>> getFile(string path);
    public Task<Result<string>> deleteFile(string path);
    public Task<Result<string>> updateFile(string path, string base64Content);

}

