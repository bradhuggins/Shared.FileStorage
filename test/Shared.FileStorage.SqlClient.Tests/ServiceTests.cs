using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shared.FileStorage.SqlClient;
using System.Threading.Tasks;

namespace Shared.FileStorage.SqlClientTests
{
    [TestClass]
    public class ServiceTests
    {
        private string _connectionString = "Application Name=Shared.FileStorage.SqlClientTests;Connection Timeout=15;pooling=true;Data Source=.\\SQLEXPRESS;Initial Catalog=SampleAppDb;Integrated Security=True;MultipleActiveResultSets=True;";

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
