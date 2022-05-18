using FixPolishCharactersInSubtitles.FileManagement;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using Xunit;

namespace FixPolishCharactersInSubtitlesTests
{
    public class GetLocalFilesTests
    {
        private readonly Mock<IFileSystem> _fileSystemMock;
        private readonly GetLocalFilesService _getLocalFilesService;

        public GetLocalFilesTests()
        {
            _fileSystemMock = new Mock<IFileSystem>();
            _getLocalFilesService = new GetLocalFilesService(_fileSystemMock.Object);
        }

        [Fact]
        public void GetLocalFiles_NoFilesInCurrentLocation_EmptyList()
        {
            _fileSystemMock.Setup(f => f.Path.GetDirectoryName(It.IsAny<string>())).Returns(It.IsAny<string>());
            _fileSystemMock.Setup(f => f.Directory.GetFiles(It.IsAny<string>())).Returns(Array.Empty<string>());
            _fileSystemMock.Setup(f => f.Directory.Exists(It.IsAny<string>())).Returns(true);

            List<string> files = _getLocalFilesService.GetLocalFiles();

            Assert.Empty(files);
        }

        [Fact]
        public void GetLocalFiles_SingleFileInDirectory_SingleFileNameReturned()
        {
            string[] fileName = new string[] { "filename" };
            _fileSystemMock.Setup(f => f.Path.GetDirectoryName(It.IsAny<string>())).Returns(It.IsAny<string>());
            _fileSystemMock.Setup(f => f.Directory.GetFiles(It.IsAny<string>())).Returns(fileName);
            _fileSystemMock.Setup(f => f.Directory.Exists(It.IsAny<string>())).Returns(true);

            List<string> files = _getLocalFilesService.GetLocalFiles();

            Assert.Equal(fileName, files);
        }

        [Fact]
        public void GetLocalFiles_DierctoryNameNull_NullReferenceExceptionIsThrown()
        {
            Assert.Throws<NullReferenceException>(() => _getLocalFilesService.GetLocalFiles());
        }

        [Fact]
        public void GetLocalFiles_DierctoryDoesNotExist_NullReferenceExceptionIsThrown()
        {
            _fileSystemMock.Setup(f => f.Path.GetDirectoryName(It.IsAny<string>())).Returns(It.IsAny<string>());
            _fileSystemMock.Setup(f => f.Directory.Exists(It.IsAny<string>())).Returns(false);

            Assert.Throws<DirectoryNotFoundException>(() => _getLocalFilesService.GetLocalFiles());
        }
    }
}