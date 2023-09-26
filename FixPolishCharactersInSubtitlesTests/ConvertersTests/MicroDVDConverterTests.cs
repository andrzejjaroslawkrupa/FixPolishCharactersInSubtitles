using FixPolishCharactersInSubtitles.CharacterTranslation.Converters;
using System;
using Xunit;

namespace FixPolishCharactersInSubtitlesTests.ConvertersTests
{
    public class MicroDVDConverterTests
    {
        private readonly MicroDVDConverter _converter;
        public MicroDVDConverterTests()
        {
            _converter = new MicroDVDConverter();
        }

        [Theory]
        [InlineData("{1}{1}23.976", "1\r\n00:00:00,041 --> 00:00:00,041\r\n23.976\r\n\r\n")]
        [InlineData("{1}{1}line1", "1\r\n00:00:00,041 --> 00:00:00,041\r\nline1\r\n\r\n")]
        [InlineData("{1}{1}5", "1\r\n00:00:00,041 --> 00:00:00,041\r\n5\r\n\r\n")]
        [InlineData("{1}{1}300", "1\r\n00:00:00,041 --> 00:00:00,041\r\n300\r\n\r\n")]
        [InlineData("{1}{1}23.976\r\n{5055}{5066}line2\r\n{5515}{5583}ąćęłńśżźĄĆĘŁŃŚŻŹ",
            "1\r\n00:00:00,041 --> 00:00:00,041\r\n23.976\r\n\r\n2\r\n00:03:30,835 --> 00:03:31,294\r\nline2\r\n\r\n3\r\n00:03:50,021 --> 00:03:52,857\r\nąćęłńśżźĄĆĘŁŃŚŻŹ\r\n\r\n")]
        [InlineData("{5055}{5066}line1\r\n{5515}", "1\r\n00:03:30,625 --> 00:03:31,083\r\nline1\r\n\r\n")]
        public void ConvertToSubRip_MicroDVDContent_ContentConveertedToSubRip(string input, string output)
        {
            input = input.Replace("\r\n", Environment.NewLine);
            output = output.Replace("\r\n", Environment.NewLine);

            var result = _converter.ConvertToSubRip(input);

            Assert.Equal(output, result);
        }

        [Fact]
        public void ConvertToSubRip_Null_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _converter.ConvertToSubRip(null));
        }

        [Fact]
        public void ConvertToSubRip_EmptyString_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _converter.ConvertToSubRip(string.Empty));
        }
    }
}
