#region Using Statements
using System.Collections.Generic;
using System.Threading.Tasks; 
#endregion

namespace Shared.FileStorage
{
    public interface IService
    {
        string ErrorMessage { get; set; }
        bool HasError { get; }
        string ConnectionString { get; set; }

        Task DeleteFile(string shareName, string folder, string fileName);
        Task<byte[]> DownloadFile(string shareName, string folder, string fileName);
        Task UploadFile(string shareName, string folder, string fileName, byte[] fileData);
        Task<List<string>> ListFolders(string shareName);
        Task<List<string>> ListFiles(string shareName, string folder);
    }
}
