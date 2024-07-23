using FixPolishCharactersInSubtitles.CharacterTranslation.Converters;
using System;
using Xunit;

namespace FixPolishCharactersInSubtitlesTests.ConvertersTests
{
    public class ConverterFactoryTests
    {
        private readonly ConverterFactory _converterFactory;

        public ConverterFactoryTests()
        {
            _converterFactory = new ConverterFactory();
        }

        [Fact]
        public void ConvertContentToSrt_RandomText_FormatExceptionThrown()
        {
            Assert.Throws<FormatException>(() => _converterFactory.CreateConverter("random text"));
        }

        [Fact]
        public void ConvertContentToSrt_EmptyInput_FormatExceptionThrown()
        {
            Assert.Throws<FormatException>(() => _converterFactory.CreateConverter(string.Empty));
        }

        [Theory]
        [InlineData("{1}{1}23.976")]
        [InlineData("{1}{1}line1")]
        [InlineData("{1}{1}5")]
        [InlineData("{1}{1}300")]
        [InlineData("{1}{1}23.976\r\n{5055}{5066}line2\r\n{5515}{5583}ąćęłńśżźĄĆĘŁŃŚŻŹ")]
        [InlineData("{5055}{5066}line1\r\n{5515}")]
        public void ConvertContentToSrt_MicroDvdFormat_MicroDvdConverter(string input)
        {
            var result = _converterFactory.CreateConverter(input);

            Assert.IsType<MicroDVDConverter>(result);
        }

        [Theory]
        [InlineData("[1][2]test")]
        [InlineData("[1][2]line1")]
        [InlineData("[1][3]5")]
        [InlineData("[1][200]300")]
        [InlineData("[1][33]test\r\n[1736][1776]line2\r\n[1776][1815]ąćęłńśżźĄĆĘŁŃŚŻŹ")]
        [InlineData("[1736][1776]line1\r\n[1776]")]
        public void ConvertContentToSrt_CustomTimeStampFormat_CustomTimeStampConverter(string input)
        {
            var result = _converterFactory.CreateConverter(input);

            Assert.IsType<CustomTimeStampConverter>(result);
        }
    }
}
