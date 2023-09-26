using FixPolishCharactersInSubtitles.Abstractions;
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
        private readonly Mock<IConverterFactory> _converterFactory;
        private readonly ConvertToSubRipService _convertToSrtService;

        public ConvertToSrtServiceTests()
        {
            _fileSystem = new Mock<IFileSystem>();
            _converterFactory = new Mock<IConverterFactory>();
            _convertToSrtService = new ConvertToSubRipService(_fileSystem.Object, _converterFactory.Object);
        }

        [Fact]
        public void ConvertContentToSubRip_Null_ArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _convertToSrtService.ConvertContentToSubRip(null));
        }

        [Fact]
        public void ConvertContentToSubRip_EmptyString_ArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _convertToSrtService.ConvertContentToSubRip(string.Empty));
        }

        [Fact]
        public void ConvertContentToSubRip_ValidInput_ConverterCreatedAndUsed()
        {
            var input = "test";
            var converter = new Mock<IConverter>();
            _converterFactory.Setup(c => c.CreateConverter(input)).Returns(converter.Object);

            _convertToSrtService.ConvertContentToSubRip(input);

            _converterFactory.Verify(c => c.CreateConverter(input), Times.Once);
            converter.Verify(c => c.ConvertToSubRip(input), Times.Once);
        }

        [Fact]
        public void ConvertPathToSrt_NullPath_ArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _convertToSrtService.ConvertPathToSrt(null));
        }

        [Fact]
        public void ConvertPathToSrt_EmptyPath_ArgumentNullExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _convertToSrtService.ConvertPathToSrt(string.Empty));
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
