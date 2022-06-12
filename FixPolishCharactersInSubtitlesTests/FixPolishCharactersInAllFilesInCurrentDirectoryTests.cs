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
            var filesList = new List<string> { "file1.srt", "file2.srt" };
            SetupSut(filesList);

            _fixPolishCharactersInAllFilesInCurrentDirectory.ConvertAllFiles();

            AssertFilesWereConverted(filesList);
        }

        [Fact]
        public void ConvertAllFiles_NoFiles_NoConversion()
        {
            SetupSut(new List<string>());

            _fixPolishCharactersInAllFilesInCurrentDirectory.ConvertAllFiles();

            AssertNoFilesConverted();
        }

        [Fact]
        public void ConvertAllFiles_FileWithUnsupportedExtentsion_NoConversion()
        {
            SetupSut(new List<string> { "file.unsupported" });

            _fixPolishCharactersInAllFilesInCurrentDirectory.ConvertAllFiles();

            AssertNoFilesConverted();
        }

        private void SetupSut(List<string> filesList)
        {
            _getLocalFilesMock.Setup(g => g.GetLocalFiles()).Returns(filesList);
            _fileSystemMock.Setup(f => f.File.ReadAllText(It.IsAny<string>())).Returns(string.Empty);
            _fileSystemMock.Setup(f => f.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));
        }

        private void AssertFilesWereConverted(List<string> files)
        {
            foreach (var file in files)
            {
                _fileSystemMock.Verify(f => f.File.ReadAllText(file, Encoding.GetEncoding(1252)), Times.Once);
                _fileSystemMock.Verify(f => f.File.WriteAllText(file, It.IsAny<string>(), Encoding.UTF8), Times.Once);
            }
            _translateCharactersServiceMock.Verify(t => t.Translate(It.IsAny<string>()), Times.Exactly(files.Count));
        }

        private void AssertNoFilesConverted()
        {
            _fileSystemMock.Verify(f => f.File.ReadAllText(It.IsAny<string>(), It.IsAny<Encoding>()), Times.Never);
            _translateCharactersServiceMock.Verify(t => t.Translate(It.IsAny<string>()), Times.Never);
            _fileSystemMock.Verify(f => f.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>(), Encoding.UTF8), Times.Never);
            _fileSystemMock.Verify(f => f.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>(), Encoding.UTF8), Times.Never);
        }
    }
}
