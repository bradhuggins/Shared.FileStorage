#region Using Statements
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.FileStorage.NativeClient;
using System.Threading.Tasks; 
#endregion

namespace Shared.FileStorage.NativeClientTests
{
    [TestClass]
    public class ServiceTests
    {
        private string _connectionString = @"c:\temp\fileStorageTest";

        [TestMethod]
        public async Task UploadFileTest()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;
            string filename = MockData.Utilities.GenerateNewFilename();
            // Act
            await target.UploadFile("shareName", "folder", filename, MockData.Utilities.TransparentGif);

            // Assert
            Assert.IsFalse(target.HasError);
        }

        [TestMethod]
        public async Task DownloadFileTest()
        {
            // Arrange
            Service target = new Service();
            target.ConnectionString = _connectionString;
            string filename = MockData.Utilities.GenerateNewFilename();

            // Act
            await target.UploadFile("shareName", "folder", filename, MockData.Utilities.TransparentGif);
            var actual = await target.DownloadFile("shareName", "folder", filename);

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
            string filename = MockData.Utilities.GenerateNewFilename();

            // Act
            await target.UploadFile("shareName", "folder", filename, MockData.Utilities.TransparentGif);
            await target.DeleteFile("shareName", "folder", filename);

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
            await target.UploadFile("shareName", "folder1", MockData.Utilities.GenerateNewFilename(), MockData.Utilities.TransparentGif);
            await target.UploadFile("shareName", "folder2", MockData.Utilities.GenerateNewFilename(), MockData.Utilities.TransparentGif);
            var actual = await target.ListFolders("shareName");

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
            await target.UploadFile("shareName", "folder3", MockData.Utilities.GenerateNewFilename(), MockData.Utilities.TransparentGif);
            await target.UploadFile("shareName", "folder3", MockData.Utilities.GenerateNewFilename(), MockData.Utilities.TransparentGif);
            await target.UploadFile("shareName", "folder3", MockData.Utilities.GenerateNewFilename(), MockData.Utilities.TransparentGif);
            var actual = await target.ListFiles("shareName", "folder3");

            // Assert
            Assert.IsFalse(target.HasError);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Count > 0);
        }

    }
}
