using FixPolishCharactersInSubtitles.CharacterTranslation.Converters;
using System;
using Xunit;

namespace FixPolishCharactersInSubtitlesTests
{
    public class MicroDVDConverterTests
    {
        [Fact]
        public void ConvertFromMicroDVD_Null_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => MicroDVDConverter.ConvertFromMicroDVD(null));
        }

        [Fact]
        public void ConvertFromMicroDVD_EmptyString_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => MicroDVDConverter.ConvertFromMicroDVD(string.Empty));
        }
    }
}
