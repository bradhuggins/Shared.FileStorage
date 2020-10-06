#region Using Statements
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
#endregion

namespace Shared.FileStorage.AzureBlobClient
{
    public class Client : IClient
    {
        // https://github.com/Azure/azure-storage-net/tree/master/Samples/GettingStarted/VisualStudioQuickStarts/DataBlobStorage

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

        private async Task<CloudBlockBlob> GetBlobReference(string container, string blobName)
        {
            CloudBlockBlob cloudBlockBlob = null;
            try
            {
                CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(this.ConnectionString);
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(container);
                await cloudBlobContainer.CreateIfNotExistsAsync();

                cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(blobName);

            }
            catch (StorageException exStorage)
            {
                this.ErrorMessage = exStorage.ToString();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
            return cloudBlockBlob;
        }

        public async Task UploadFile(string shareName, string folder, string fileName, byte[] fileData)
        {
            try
            {
                CloudBlockBlob cloudBlockBlob = await this.GetBlobReference(folder, fileName);
                await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
            }
            catch (StorageException exStorage)
            {
                this.ErrorMessage = exStorage.ToString();
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
                CloudBlockBlob cloudBlockBlob = await this.GetBlobReference(folder, fileName);
                MemoryStream ms = new MemoryStream();
                await cloudBlockBlob.DownloadToStreamAsync(ms);
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
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(folder);
                await cloudBlobContainer.CreateIfNotExistsAsync();

                BlobContinuationToken token = null;
                do
                {
                    BlobResultSegment resultSegment = await cloudBlobContainer.ListBlobsSegmentedAsync(token);
                    token = resultSegment.ContinuationToken;
                    foreach (IListBlobItem item in resultSegment.Results)
                    {
                        // Blob type will be CloudBlockBlob, CloudPageBlob or CloudBlobDirectory
                        if (item.GetType() == typeof(CloudBlockBlob))
                        {
                            CloudBlockBlob newItem = (CloudBlockBlob)item;
                            toReturn.Add(newItem.Name);
                        }
                    }
                } while (token != null);

            }
            catch (StorageException exStorage)
            {
                this.ErrorMessage = exStorage.ToString();
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
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                BlobContinuationToken token = null;
                do
                {
                    ContainerResultSegment resultSegment = await cloudBlobClient.ListContainersSegmentedAsync(token);
                    token = resultSegment.ContinuationToken;
                    foreach (var item in resultSegment.Results)
                    {
                        toReturn.Add(item.Name);
                    }
                } while (token != null);
            }
            catch (StorageException exStorage)
            {
                this.ErrorMessage = exStorage.ToString();
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
                CloudBlockBlob cloudBlockBlob = await this.GetBlobReference(folder, fileName);
                await cloudBlockBlob.DeleteAsync();

            }
            catch (StorageException exStorage)
            {
                this.ErrorMessage = exStorage.ToString();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.ToString();
            }
        }
    }
}
