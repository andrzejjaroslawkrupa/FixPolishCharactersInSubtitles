using FixPolishCharactersInSubtitles.FileManagement;
using System.IO.Abstractions;

namespace FixPolishCharactersInSubtitles.IntegrationTests
{
    public class GetFilesServiceTests : IDisposable
    {
        private string TestDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestFiles");
        private readonly GetFilesService _getFilesService;

        public GetFilesServiceTests()
        {
            if (!Directory.Exists(TestDirectory))
                Directory.CreateDirectory(TestDirectory);
            _getFilesService = new GetFilesService(new FileSystem());
        }

        public void Dispose()
        {
            if (Directory.Exists(TestDirectory))
                Directory.Delete(TestDirectory, true);
        }

        [Fact]
        public void GetFilesFromDir_NoFiles_ReturnsEmptyList()
        {
            List<string> filesList = _getFilesService.GetFilesFromDir(TestDirectory);

            Assert.Empty(filesList);
        }

        [Fact]
        public void GetFilesFromDir_NoDirectory_ReturnsEmptyList()
        {
            Directory.Delete(TestDirectory, true);

            List<string> filesList = _getFilesService.GetFilesFromDir(TestDirectory);

            Assert.Empty(filesList);
        }

        [Fact]
        public void GetFilesFromDir_OneFile_ReturnsOneFile()
        {
            var path = Path.Combine(TestDirectory, "testfile.txt");
            using (var writer = new StreamWriter(path))
            {
                writer.Write(string.Empty);
            }

            List<string> filesList = _getFilesService.GetFilesFromDir(TestDirectory);

            Assert.Single(filesList, path);
        }

        [Fact]
        public void GetFilesFromDir_TwoFiles_ReturnsTwoFiles()
        {
            var pathOne = Path.Combine(TestDirectory, "testfile1.txt");
            var pathTwo = Path.Combine(TestDirectory, "testfile2.jpg");
            using (var writer = new StreamWriter(pathOne))
            {
                writer.Write(string.Empty);
            }
            using (var writer = new StreamWriter(pathTwo))
            {
                writer.Write(string.Empty);
            }

            List<string> filesList = _getFilesService.GetFilesFromDir(TestDirectory);

            Assert.Contains(pathOne, filesList);
            Assert.Contains(pathTwo, filesList);
        }
    }
}