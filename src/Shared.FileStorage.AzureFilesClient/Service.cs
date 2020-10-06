#region Using Statements
//using Azure;
//using Azure.Storage.Files.Shares;
//using Azure.Storage.Files.Shares.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
#endregion

namespace Shared.FileStorage.AzureFilesClient
{
    public class Service : IService
    {
        public string BasePath { get; set; }

        public string ErrorMessage { get; set; }

        public bool HasError
        {
            get { return !string.IsNullOrEmpty(this.ErrorMessage); }
        }

        private string _connectionString;

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    throw new System.Exception("Error: ConnectionString not set!");
                }
                return _connectionString;
            }
            set { _connectionString = value; }
        }

        #region V12
        //private async Task<ShareClient> GetShareReference(string shareName)
        //{
        //    // Instantiate a ShareClient which will be used to create and manipulate the file share
        //    ShareClient share = new ShareClient(this.ConnectionString, shareName);

        //    // Create the share if it doesn't already exist
        //    await share.CreateIfNotExistsAsync();

        //    return share;
        //}

        //private async Task<ShareDirectoryClient> GetDirectoryReference(string shareName, string folder)
        //{
        //    ShareDirectoryClient directory = null;
        //    ShareClient share = await this.GetShareReference(shareName);
        //    if (await share.ExistsAsync())
        //    {
        //        // Get a reference to the sample directory
        //        directory = share.GetDirectoryClient(folder);

        //        // Create the directory if it doesn't already exist
        //        await directory.CreateIfNotExistsAsync();
        //    }
        //    return directory;
        //}

        //public async Task DeleteFile(string shareName, string folder, string fileName)
        //{
        //    try
        //    {
        //        ShareDirectoryClient directory = await this.GetDirectoryReference(shareName, folder);
        //        // Ensure that the directory exists
        //        if (await directory.ExistsAsync())
        //        {
        //            // Get a reference to a file object
        //            ShareFileClient file = directory.GetFileClient(fileName);
        //            // Ensure that the file exists
        //            if (await file.ExistsAsync())
        //            {
        //                await file.DeleteAsync();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.ErrorMessage = ex.ToString();
        //    }
        //}

        //public async Task<byte[]> DownloadFile(string shareName, string folder, string fileName)
        //{
        //    byte[] toReturn = null;
        //    try
        //    {
        //        ShareDirectoryClient directory = await this.GetDirectoryReference(shareName, folder);
        //        // Ensure that the directory exists
        //        if (await directory.ExistsAsync())
        //        {
        //            // Get a reference to a file object
        //            ShareFileClient file = directory.GetFileClient(fileName);
        //            // Ensure that the file exists
        //            if (await file.ExistsAsync())
        //            {
        //                // Download the file
        //                ShareFileDownloadInfo download = await file.DownloadAsync();
        //                MemoryStream ms = new MemoryStream();
        //                await download.Content.CopyToAsync(ms);
        //                toReturn = ms.ToArray();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.ErrorMessage = ex.ToString();
        //    }
        //    return toReturn;
        //}

        //public async Task UploadFile(string shareName, string folder, string fileName, byte[] fileData)
        //{
        //    try
        //    {
        //        ShareDirectoryClient directory = await this.GetDirectoryReference(shareName, folder);
        //        // Ensure that the directory exists
        //        if (await directory.ExistsAsync())
        //        {
        //            // Get a reference to a file and upload it
        //            ShareFileClient file = directory.GetFileClient(fileName);

        //            MemoryStream stream = new MemoryStream(fileData);

        //            await file.CreateAsync(stream.Length);
        //            await file.UploadRangeAsync(
        //                new HttpRange(0, stream.Length),
        //                stream);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.ErrorMessage = ex.ToString();
        //    }
        //}

        //public async Task<List<string>> ListFolders(string shareName)
        //{
        //    List<string> toReturn = new List<string>();
        //    try
        //    {
        //        ShareClient share = await this.GetShareReference(shareName);
        //        if (await share.ExistsAsync())
        //        {
        //            // Track the remaining directories to walk, starting from the root
        //            var remaining = new Queue<ShareDirectoryClient>();
        //            remaining.Enqueue(share.GetRootDirectoryClient());
        //            while (remaining.Count > 0)
        //            {
        //                // Get all of the next directory's files and subdirectories
        //                ShareDirectoryClient dir = remaining.Dequeue();
        //                foreach (ShareFileItem item in dir.GetFilesAndDirectories())
        //                {
        //                    // Keep walking down directories
        //                    if (item.IsDirectory)
        //                    {
        //                        toReturn.Add(item.Name);
        //                        //remaining.Enqueue(dir.GetSubdirectoryClient(item.Name));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.ErrorMessage = ex.ToString();
        //    }
        //    return toReturn;
        //}

        //public async Task<List<string>> ListFiles(string shareName, string folder)
        //{
        //    List<string> toReturn = new List<string>();
        //    try
        //    {
        //        ShareDirectoryClient directory = await this.GetDirectoryReference(shareName, folder);
        //        if (await directory.ExistsAsync())
        //        {
        //            // Track the remaining directories to walk, starting from the root
        //            var remaining = new Queue<ShareDirectoryClient>();
        //            remaining.Enqueue(directory);
        //            while (remaining.Count > 0)
        //            {
        //                // Get all of the next directory's files and subdirectories
        //                ShareDirectoryClient dir = remaining.Dequeue();
        //                foreach (ShareFileItem item in dir.GetFilesAndDirectories())
        //                {
        //                    if (!item.IsDirectory)
        //                    {
        //                        toReturn.Add(item.Name);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.ErrorMessage = ex.ToString();
        //    }
        //    return toReturn;
        //} 
        #endregion

        #region V11
        private CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount = null;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                this.ErrorMessage = "Invalid storage account information provided in config.";

            }
            catch (ArgumentException)
            {
                this.ErrorMessage = "Invalid storage account information provided in config.";
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return storageAccount;
        }

        private async Task<CloudFile> GetFileReference(string shareName, string folder, string fileName)
        {
            CloudFile cloudFile = null;
            try
            {
                CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(this.ConnectionString);
                CloudFileClient cloudFileClient = storageAccount.CreateCloudFileClient();

                CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(shareName);
                await cloudFileShare.CreateIfNotExistsAsync();

                CloudFileDirectory rootDirectory = cloudFileShare.GetRootDirectoryReference();
                CloudFileDirectory fileDirectory = null;
                if (!string.IsNullOrEmpty(folder))
                {
                    fileDirectory = rootDirectory.GetDirectoryReference(folder);
                }
                else
                {
                    fileDirectory = rootDirectory;
                }
                await fileDirectory.CreateIfNotExistsAsync();
                cloudFile = fileDirectory.GetFileReference(fileName);

            }
            catch (StorageException exStorage)
            {
                this.ErrorMessage = exStorage.ToString();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return cloudFile;
        }

        public async Task UploadFile(string shareName, string folder, string fileName, byte[] fileData)
        {
            try
            {
                CloudFile cloudFile = await this.GetFileReference(shareName, folder, fileName);
                await cloudFile.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
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
                CloudFile cloudFile = await this.GetFileReference(shareName, folder, fileName);
                MemoryStream ms = new MemoryStream();
                await cloudFile.DownloadToStreamAsync(ms);
                toReturn = ms.ToArray();
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
                CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(this.ConnectionString);
                CloudFileClient cloudFileClient = storageAccount.CreateCloudFileClient();

                CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(shareName);
                await cloudFileShare.CreateIfNotExistsAsync();

                CloudFileDirectory rootDirectory = cloudFileShare.GetRootDirectoryReference();
                CloudFileDirectory fileDirectory = null;
                if (!string.IsNullOrEmpty(folder))
                {
                    fileDirectory = rootDirectory.GetDirectoryReference(folder);
                }
                else
                {
                    fileDirectory = rootDirectory;
                }
                await fileDirectory.CreateIfNotExistsAsync();

                List<IListFileItem> results = new List<IListFileItem>();
                FileContinuationToken token = null;
                do
                {
                    FileResultSegment resultSegment = await fileDirectory.ListFilesAndDirectoriesSegmentedAsync(token);
                    results.AddRange(resultSegment.Results);
                    token = resultSegment.ContinuationToken;
                }
                while (token != null);

                foreach (var item in results)
                {
                    if (item.GetType() == typeof(CloudFile))
                    {
                        CloudFile file = (CloudFile)item;
                        toReturn.Add(file.Name);
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
                CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(this.ConnectionString);
                CloudFileClient cloudFileClient = storageAccount.CreateCloudFileClient();

                if (!string.IsNullOrEmpty(shareName))
                {
                    CloudFileShare cloudFileShare = cloudFileClient.GetShareReference(shareName);
                    await cloudFileShare.CreateIfNotExistsAsync();

                    CloudFileDirectory rootDirectory = cloudFileShare.GetRootDirectoryReference();
                    CloudFileDirectory fileDirectory = rootDirectory;
                    await fileDirectory.CreateIfNotExistsAsync();

                    List<IListFileItem> results = new List<IListFileItem>();
                    FileContinuationToken token = null;
                    do
                    {
                        FileResultSegment resultSegment = await fileDirectory.ListFilesAndDirectoriesSegmentedAsync(token);
                        results.AddRange(resultSegment.Results);
                        token = resultSegment.ContinuationToken;
                    }
                    while (token != null);

                    foreach (var item in results)
                    {
                        if (item.GetType() == typeof(CloudFileDirectory))
                        {
                            CloudFileDirectory folder = (CloudFileDirectory)item;
                            toReturn.Add(folder.Name);
                        }
                    }
                }
                else
                {
                    List<CloudFileShare> results = new List<CloudFileShare>();
                    FileContinuationToken token = null;
                    do
                    {
                        ShareResultSegment resultSegment = await cloudFileClient.ListSharesSegmentedAsync(token);
                        results.AddRange(resultSegment.Results);
                        token = resultSegment.ContinuationToken;
                    }
                    while (token != null);

                    foreach (var item in results)
                    {
                        if (item.GetType() == typeof(CloudFileShare))
                        {
                            CloudFileShare share = (CloudFileShare)item;
                            toReturn.Add(share.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return toReturn;
        }

        public async Task DeleteFile(string shareName, string folder, string fileName)
        {
            try
            {
                CloudFile cloudFile = await this.GetFileReference(shareName, folder, fileName);
                await cloudFile.DeleteAsync();

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
        }
        #endregion
    }
}
