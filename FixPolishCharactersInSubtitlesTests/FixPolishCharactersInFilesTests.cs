using FixPolishCharactersInSubtitles.Abstractions;
using FixPolishCharactersInSubtitles.CharacterTranslation;
using FixPolishCharactersInSubtitles.FileManagement;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using Xunit;

namespace FixPolishCharactersInSubtitlesTests
{
    public class FixPolishCharactersInFilesTests
    {
        private readonly Mock<IGetFiles> _getFilesMock;
        private readonly Mock<ITranslateCharactersService> _translateCharactersServiceMock;
        private readonly Mock<IConvertToSubRipService> _convertToSrtServiceMock;
        private readonly Mock<IFileSystem> _fileSystemMock;
        private readonly Mock<ICommandLineInterface> _commandLineInterfaceMock;
        private readonly FixPolishCharactersInFilesI _fixPolishCharactersInFiles;

        public FixPolishCharactersInFilesTests()
        {
            _getFilesMock = new Mock<IGetFiles>();
            _translateCharactersServiceMock = new Mock<ITranslateCharactersService>();
            _convertToSrtServiceMock = new Mock<IConvertToSubRipService>();
            _fileSystemMock = new Mock<IFileSystem>();
            _commandLineInterfaceMock = new Mock<ICommandLineInterface>();
            _fixPolishCharactersInFiles = new FixPolishCharactersInFilesI(
                _getFilesMock.Object, _translateCharactersServiceMock.Object, _convertToSrtServiceMock.Object, _fileSystemMock.Object, _commandLineInterfaceMock.Object);
        }

        [Theory]
        [MemberData(nameof(InputFileListTestData))]
        public void Convert_ValidListOfFiles_AllFilesAreConverted(List<string> filesList)
        {
            SetupSut(filesList);

            _fixPolishCharactersInFiles.Convert();

            AssertFilesWereConverted(filesList);
        }

        public static IEnumerable<object[]> InputFileListTestData()
        {
            yield return new object[] { new List<string> { "file1.srt", "file2.srt" } };
            yield return new object[] { new List<string> { "file1.txt", "file2.txt" } };
            yield return new object[] { new List<string> { "file1.srt", "file2.txt" } };
            yield return new object[] { new List<string> { "file1.txt", "file2.srt" } };
            yield return new object[] { new List<string> { "file1.txt" } };
            yield return new object[] { new List<string> { "file1.srt" } };
            yield return new object[] { new List<string> { "file1.txt", "file2.srt", "file3.srt" } };
            yield return new object[] { new List<string> { "file1.srt", "file2.srt", "file3.srt" } };
            yield return new object[] { new List<string> { "file1.srt", "file2.txt", "file3.srt" } };
            yield return new object[] { new List<string> { "file1.srt", "file2.srt", "file3.txt" } };
        }

        [Fact]
        public void Convert_NoFiles_NoConversion()
        {
            SetupSut(new List<string>());

            _fixPolishCharactersInFiles.Convert();

            AssertNoFilesConverted();
        }

        [Fact]
        public void Convert_FileWithUnsupportedExtentsion_NoConversion()
        {
            SetupSut(new List<string> { "file.unsupported" });

            _fixPolishCharactersInFiles.Convert();

            AssertNoFilesConverted();
        }

        [Fact]
        public void Convert_TxtFile_FileConvertedToSrt()
        {
            var filename = "file.txt";
            SetupSut(new List<string> { filename });

            _fixPolishCharactersInFiles.Convert();

            _convertToSrtServiceMock.Verify(m => m.ConvertContentToSubRip(It.IsAny<string>()), Times.Once);
            _convertToSrtServiceMock.Verify(c => c.ConvertPathToSrt(filename), Times.Once);
        }

        [Fact]
        public void Convert_SrtFile_FileNotConvertedToSrt()
        {
            var filename = "file.srt";
            SetupSut(new List<string> { filename });

            _fixPolishCharactersInFiles.Convert();

            _convertToSrtServiceMock.Verify(m => m.ConvertContentToSubRip(It.IsAny<string>()), Times.Never);
            _convertToSrtServiceMock.Verify(c => c.ConvertPathToSrt(filename), Times.Never);
        }

        [Fact]
        public void Convert_ValidPathArgument_FileConverted()
        {
            var file = new List<string> { "file.txt" };
            _commandLineInterfaceMock.Setup(c => c.GetCommandLineArgs()).Returns(new string[] { "--path", "file.txt" });
            SetupSut(file);

            _fixPolishCharactersInFiles.Convert();

            AssertFilesWereConverted(file);
        }

        [Fact]
        public void Convert_EmptyPathArgument_ExceptionIsThrown()
        {
            _commandLineInterfaceMock.Setup(c => c.GetCommandLineArgs()).Returns(new string[] { "--path", "" });

            Assert.Throws<ArgumentNullException>(() => _fixPolishCharactersInFiles.Convert());
        }

        [Fact]
        public void Convert_NullPathArgument_ExceptionIsThrown()
        {
            _commandLineInterfaceMock.Setup(c => c.GetCommandLineArgs()).Returns(new string[] { "--path", null });

            Assert.Throws<ArgumentNullException>(() => _fixPolishCharactersInFiles.Convert());
        }

        [Fact]
        public void Convert_PathFromArgumentDoesNotExist_ExceptionIsThrown()
        {
            _commandLineInterfaceMock.Setup(c => c.GetCommandLineArgs()).Returns(new string[] { "--path", "fake\\path"});
            _fileSystemMock.Setup(f => f.Directory.Exists(It.IsAny<string>())).Returns(false);

            Assert.Throws<DirectoryNotFoundException>(() => _fixPolishCharactersInFiles.Convert());
        }

        private void SetupSut(List<string> filesList)
        {
            foreach (var file in filesList)
            {
                _fileSystemMock.Setup(f => f.Directory.Exists(file)).Returns(true);
            }
            _getFilesMock.Setup(g => g.GetLocalFiles()).Returns(filesList);
            _getFilesMock.Setup(g => g.GetFilesFromDir(It.IsAny<string>())).Returns(filesList);
            _fileSystemMock.Setup(f => f.File.ReadAllText(It.IsAny<string>())).Returns(string.Empty);
            _fileSystemMock.Setup(f => f.File.WriteAllText(It.IsAny<string>(), It.IsAny<string>()));
        }

        private void AssertFilesWereConverted(List<string> files)
        {
            foreach (var file in files)
            {
                string? fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                _fileSystemMock.Verify(f => f.File.ReadAllText(file, Encoding.GetEncoding(1252)), Times.Once);
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
