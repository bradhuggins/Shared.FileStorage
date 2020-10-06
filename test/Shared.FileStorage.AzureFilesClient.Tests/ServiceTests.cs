using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.FileStorage.AzureFilesClient;
using System.Threading.Tasks;

namespace Shared.FileStorage.AzureFilesClientTests
{
    [TestClass]
    public class ServiceTests
    {
        private string _connectionString = "UseDevelopmentStorage=true;";

        [TestMethod]
        public async Task UploadFileTest()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;
            string filename = MockData.Utilities.GenerateNewFilename().ToLower();
            // Act
            await target.UploadFile("sharename", "folder", filename, MockData.Utilities.TransparentGif);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public async Task DownloadFileTest()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;
            string filename = MockData.Utilities.GenerateNewFilename().ToLower();

            // Act
            await target.UploadFile("sharename", "folder", filename, MockData.Utilities.TransparentGif);
            var actual = await target.DownloadFile("sharename", "folder", filename);

            // Assert
            Assert.IsFalse(target.HasError);
            Assert.IsNotNull(actual);

        }

        [TestMethod]
        public async Task DeleteFileTest()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;
            string filename = MockData.Utilities.GenerateNewFilename().ToLower();

            // Act
            await target.UploadFile("sharename", "folder", filename, MockData.Utilities.TransparentGif);
            await target.DeleteFile("sharename", "folder", filename);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public async Task ListFoldersTest()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            // Act
            await target.UploadFile("sharename", "folder1", MockData.Utilities.GenerateNewFilename(), MockData.Utilities.TransparentGif);
            await target.UploadFile("sharename", "folder2", MockData.Utilities.GenerateNewFilename(), MockData.Utilities.TransparentGif);
            var actual = await target.ListFolders("sharename");

            // Assert
            Assert.IsFalse(target.HasError);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
        }

        [TestMethod]
        public async Task ListFilesTest()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;

            // Act
            await target.UploadFile("sharename", "folder3", MockData.Utilities.GenerateNewFilename(), MockData.Utilities.TransparentGif);
            await target.UploadFile("sharename", "folder3", MockData.Utilities.GenerateNewFilename(), MockData.Utilities.TransparentGif);
            await target.UploadFile("sharename", "folder3", MockData.Utilities.GenerateNewFilename(), MockData.Utilities.TransparentGif);
            var actual = await target.ListFiles("sharename", "folder3");

            // Assert
            Assert.IsFalse(target.HasError);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
        }

    }
}
