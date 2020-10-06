#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace Shared.FileStorage.NativeClient
{
    public class Service : IService
    {
        private string _basePath;

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_basePath))
                {
                    throw new System.Exception("Error: BasePath not set!");
                }
                return _basePath;
            }
            set { _basePath = value; }
        }

        public string ErrorMessage { get; set; }

        public bool HasError
        {
            get { return !string.IsNullOrEmpty(this.ErrorMessage); }
        }

        public async Task DeleteFile(string shareName, string folder, string fileName)
        {
            try
            {
                string completeFilename = BuildFullFilename(shareName, folder, fileName);
                if (File.Exists(completeFilename))
                {
                    File.Delete(completeFilename);
                }
                else
                {
                    this.ErrorMessage = "File not found. " + completeFilename;
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
        }

        public async Task<byte[]> DownloadFile(string shareName, string folder, string fileName)
        {
            byte[] toReturn = null;
            try
            {
                string completeFilename = BuildFullFilename(shareName, folder, fileName);
                if (File.Exists(completeFilename))
                {
                    toReturn = File.ReadAllBytes(completeFilename);
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return toReturn;
        }

        public async Task<List<string>> ListFiles(string shareName, string folder)
        {
            List<string> toReturn = new List<string>();
            try
            {
                string fullPath = BuildFullFilename(shareName, folder, null);
                if (Directory.Exists(fullPath))
                {
                    toReturn = Directory.GetFiles(fullPath).ToList();
                    for (int i = 0; i < toReturn.Count; i++)
                    {
                        toReturn[i] = toReturn[i].Replace(fullPath, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return toReturn;
        }

        public async Task<List<string>> ListFolders(string shareName)
        {
            List<string> toReturn = new List<string>();
            try
            {
                string fullPath = BuildFullFilename(shareName, null, null);
                if (Directory.Exists(fullPath))
                {
                    toReturn = Directory.GetDirectories(fullPath).ToList();
                    for (int i = 0; i < toReturn.Count; i++)
                    {
                        toReturn[i] = toReturn[i].Replace(fullPath, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return toReturn;
        }

        public async Task UploadFile(string shareName, string folder, string fileName, byte[] fileData)
        {
            try
            {
                string completeFilename = BuildFullFilename(shareName, folder, fileName);
                if (!Directory.Exists(Path.GetDirectoryName(completeFilename)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFilename));
                }
                File.WriteAllBytes(completeFilename, fileData);
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
        }

        private string BuildFullFilename(string shareName, string folder, string fileName)
        {
            string toReturn = _basePath;
            if (!toReturn.EndsWith(@"\"))
            {
                toReturn += @"\";
            }
            if (!string.IsNullOrEmpty(shareName))
            {
                toReturn += shareName + @"\";
            }

            if (!string.IsNullOrEmpty(folder))
            {
                toReturn += folder + @"\";
            }
            toReturn += fileName;
            return toReturn;
        }
    }
}
