using FixPolishCharactersInSubtitles.CharacterTranslation.Converters;
using System;
using Xunit;

namespace FixPolishCharactersInSubtitlesTests.ConvertersTests
{
    public class CustomTimeStampConverterTests
    {
        private readonly CustomTimeStampConverter _converter;
        public CustomTimeStampConverterTests()
        {
            _converter = new CustomTimeStampConverter();
        }

        [Theory]
        [InlineData("[1][2]test", "1\r\n00:00:00,100 --> 00:00:00,200\r\ntest")]
        [InlineData("[1][3]line1", "1\r\n00:00:00,100 --> 00:00:00,300\r\nline1")]
        [InlineData("[2][3]5", "1\r\n00:00:00,200 --> 00:00:00,300\r\n5")]
        [InlineData("[1][1]300", "1\r\n00:00:00,100 --> 00:00:00,100\r\n300")]
        [InlineData("[1][1]test\r\n[1736][1776]line2\r\n[1776][1815]ąćęłńśżźĄĆĘŁŃŚŻŹ",
            "1\r\n00:00:00,100 --> 00:00:00,100\r\ntest\r\n\r\n2\r\n00:02:53,600 --> 00:02:57,600\r\nline2\r\n\r\n3\r\n00:02:57,600 --> 00:03:01,500\r\nąćęłńśżźĄĆĘŁŃŚŻŹ")]
        [InlineData("[1736][1776]line1\r\n[1776]", "1\r\n00:02:53,600 --> 00:02:57,600\r\nline1")]
        public void ConvertToSubRip_CustomTimeStampContent_ContentConvertedToSubRip(string input, string output)
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
