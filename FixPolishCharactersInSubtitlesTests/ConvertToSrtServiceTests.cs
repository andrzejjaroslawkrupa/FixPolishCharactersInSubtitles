using FixPolishCharactersInSubtitles.CharacterTranslation;
using Moq;
using System;
using System.IO;
using System.IO.Abstractions;
using Xunit;

namespace FixPolishCharactersInSubtitlesTests
{
    public class ConvertToSrtServiceTests
    {
        private readonly Mock<IFileSystem> _fileSystem;
        private readonly ConvertToSubRipService _convertToSrtService;

        public ConvertToSrtServiceTests()
        {
            _fileSystem = new Mock<IFileSystem>();
            _convertToSrtService = new ConvertToSubRipService(_fileSystem.Object);
        }

        [Theory]
        [InlineData("{1}{1}23.976", "1\r\n00:00:00,041 --> 00:00:00,041\r\n23.976\r\n\r\n")]
        [InlineData("{1}{1}line1", "1\r\n00:00:00,041 --> 00:00:00,041\r\nline1\r\n\r\n")]
        [InlineData("{1}{1}5", "1\r\n00:00:00,041 --> 00:00:00,041\r\n5\r\n\r\n")]
        [InlineData("{1}{1}300", "1\r\n00:00:00,041 --> 00:00:00,041\r\n300\r\n\r\n")]
        [InlineData("{1}{1}23.976\r\n{5055}{5066}line2\r\n{5515}{5583}ąćęłńśżźĄĆĘŁŃŚŻŹ",
            "1\r\n00:00:00,041 --> 00:00:00,041\r\n23.976\r\n\r\n2\r\n00:03:30,835 --> 00:03:31,294\r\nline2\r\n\r\n3\r\n00:03:50,021 --> 00:03:52,857\r\nąćęłńśżźĄĆĘŁŃŚŻŹ\r\n\r\n")]
        [InlineData("{5055}{5066}line1\r\n{5515}", "1\r\n00:03:30,625 --> 00:03:31,083\r\nline1\r\n\r\n")]
        public void ConvertContentToSrt_MicroDVDContent_MicroDvdConverterUsed(string input, string output)
        {
            input = input.Replace("\r\n", Environment.NewLine);
            var result = _convertToSrtService.ConvertContentToSubRip(input);

            Assert.Equal(output, result);
        }

        [Fact]
        public void ConvertContentToSrt_RandomText_FormatExceptionThrown()
        {
            Assert.Throws<FormatException>(() => _convertToSrtService.ConvertContentToSubRip("random text"));
        }

        [Fact]
        public void ConvertContentToSrt_EmptyInput_FormatExceptionThrown()
        {
            Assert.Throws<FormatException>(() => _convertToSrtService.ConvertContentToSubRip(string.Empty));
        }

        [Fact]
        public void ConvertPathToSrt_NullDirectory_NullReferenceExceptionIsThrown()
        {
            var inputDir = @"\test\dir\file.txt";

            Assert.Throws<NullReferenceException>(() => _convertToSrtService.ConvertPathToSrt(inputDir));
        }

        [Fact]
        public void ConvertPathToSrt_DirectoryDoesNotExist_DirectoryNotFoundExceptionIsThrown()
        {
            var inputDir = @"\test\dir\file.txt";
            _fileSystem.Setup(f => f.Path.GetDirectoryName(inputDir)).Returns(@"\test\dir");
            _fileSystem.Setup(f => f.Directory.Exists(@"\test\dir")).Returns(false);

            Assert.Throws<DirectoryNotFoundException>(() => _convertToSrtService.ConvertPathToSrt(inputDir));
        }

        [Fact]
        public void ConvertPathToSrt_NullFileWithoutName_NullReferenceExceptionIsThrown()
        {
            var inputDir = @"\test\dir\file.txt";
            _fileSystem.Setup(f => f.Path.GetDirectoryName(inputDir)).Returns(@"\test\dir");
            _fileSystem.Setup(f => f.Directory.Exists(@"\test\dir")).Returns(true);

            Assert.Throws<NullReferenceException>(() => _convertToSrtService.ConvertPathToSrt(inputDir));
        }
    }
}
