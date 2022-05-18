using FixPolishCharactersInSubtitles.Abstractions;
using FixPolishCharactersInSubtitles.CharacterTranslation;
using FixPolishCharactersInSubtitles.FileManagement;
using Moq;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Text;
using Xunit;

namespace FixPolishCharactersInSubtitlesTests
{
    public class FixPolishCharactersInAllFilesInCurrentDirectoryTests
    {
        private readonly Mock<IGetLocalFiles> _getLocalFilesMock;
        private readonly Mock<ITranslateCharactersService> _translateCharactersServiceMock;
        private readonly Mock<IFileSystem> _fileSystemMock;
        private readonly FixPolishCharactersInAllFilesInCurrentDirectory _fixPolishCharactersInAllFilesInCurrentDirectory;

        public FixPolishCharactersInAllFilesInCurrentDirectoryTests()
        {
            _getLocalFilesMock = new Mock<IGetLocalFiles>();
            _translateCharactersServiceMock = new Mock<ITranslateCharactersService>();
            _fileSystemMock = new Mock<IFileSystem>();
            _fixPolishCharactersInAllFilesInCurrentDirectory = new FixPolishCharactersInAllFilesInCurrentDirectory(
                _getLocalFilesMock.Object, _translateCharactersServiceMock.Object, _fileSystemMock.Object);
        }

        [Fact]
        public void ConvertAllFiles_ValidListOfFiles_AllFilesAreConverted()
        {
            _getLocalFilesMock.Setup(g => g.GetLocalFiles()).Returns(new List<string> { "file1.srt", "file2.srt" });
            _fileSystemMock.Setup(f => f.File.ReadAllText(It.IsAny<string>())).Returns(string.Empty);
            _fileSystemMock.Setup(f => f.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));
            
            _fixPolishCharactersInAllFilesInCurrentDirectory.ConvertAllFiles();

            _fileSystemMock.Verify(f => f.File.ReadAllText("file1.srt", Encoding.GetEncoding(1252)), Times.Once);
            _fileSystemMock.Verify(f => f.File.ReadAllText("file2.srt", Encoding.GetEncoding(1252)), Times.Once);
            _translateCharactersServiceMock.Verify(t => t.Translate(It.IsAny<string>()), Times.Exactly(2));
            _fileSystemMock.Verify(f => f.File.WriteAllText("file1.srt", It.IsAny<string>(), Encoding.UTF8), Times.Once);
            _fileSystemMock.Verify(f => f.File.WriteAllText("file2.srt", It.IsAny<string>(), Encoding.UTF8), Times.Once);
        }
    }
}
