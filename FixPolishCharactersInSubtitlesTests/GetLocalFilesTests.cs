using FixPolishCharactersInSubtitles.FileManagement;
using Moq;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using Xunit;

namespace FixPolishCharactersInSubtitlesTests
{
    public class GetLocalFilesTests
    {
        Mock<IFileSystem> _fileSystemMock;

        public GetLocalFilesTests()
        {
            _fileSystemMock = new Mock<IFileSystem>();
        }

        [Fact]
        public void GetLocalFiles_NoFilesInCurrentLocation_EmptyList()
        {
            _fileSystemMock.Setup(f => f.Path.GetDirectoryName(It.IsAny<string>())).Returns(It.IsAny<string>());
            _fileSystemMock.Setup(f => f.Directory.GetFiles(It.IsAny<string>())).Returns(Array.Empty<string>());
            _fileSystemMock.Setup(f => f.Directory.Exists(It.IsAny<string>())).Returns(true);
            var getLocalFilesService = new GetLocalFilesService(_fileSystemMock.Object);

            List<string> files = getLocalFilesService.GetLocalFiles();

            Assert.Empty(files);
        }

        [Fact]
        public void GetLocalFiles_SingleFileInDirectory_SingleFileNameReturned()
        {
            string[] fileName = new string[] { "filename" };
            _fileSystemMock.Setup(f => f.Path.GetDirectoryName(It.IsAny<string>())).Returns(It.IsAny<string>());
            _fileSystemMock.Setup(f => f.Directory.GetFiles(It.IsAny<string>())).Returns(fileName);
            _fileSystemMock.Setup(f => f.Directory.Exists(It.IsAny<string>())).Returns(true);
            var getLocalFilesService = new GetLocalFilesService(_fileSystemMock.Object);

            List<string> files = getLocalFilesService.GetLocalFiles();

            Assert.Equal(fileName, files);
        }
    }
}