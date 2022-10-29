using FixPolishCharactersInSubtitles.CharacterTranslation;
using System;
using Xunit;

namespace FixPolishCharactersInSubtitlesTests
{
    public class TranslateAnsiCharactersToPolishServiceTests
    {
        private readonly TranslateAnsiCharactersToPolishService _translateAnsiCharactersToPolishService;
        public TranslateAnsiCharactersToPolishServiceTests()
        {
            _translateAnsiCharactersToPolishService = new TranslateAnsiCharactersToPolishService();
        }

        [Fact]
        public void Translate_EmptyString_EmptyString()
        {
            var result = _translateAnsiCharactersToPolishService.Translate(string.Empty);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void Translate_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _translateAnsiCharactersToPolishService.Translate(null));
        }

        [Fact]
        public void Translate_TextWithoutSpecialCharacters_OutputTextIdenticalToInputText()
        {
            var text = "abc";
            var result = _translateAnsiCharactersToPolishService.Translate(text);

            Assert.Equal(text, result);
        }

        [Fact]
        public void Translate_TextWithAllSpecialCharacters_OutputTextTranslatedCorectly()
        {
            var input = "¹æê³ñœ¿Ÿ¥ÆÊ£ÑŒ¯\u008f";
            var output = "ąćęłńśżźĄĆĘŁŃŚŻŹ";
            var result = _translateAnsiCharactersToPolishService.Translate(input);

            Assert.Equal(output, result);
        }

        [Theory]
        [InlineData("¹", "ą")]
        [InlineData("æ", "ć")]
        [InlineData("ê", "ę")]
        [InlineData("³", "ł")]
        [InlineData("ñ", "ń")]
        [InlineData("œ", "ś")]
        [InlineData("¿", "ż")]
        [InlineData("Ÿ", "ź")]
        [InlineData("¥", "Ą")]
        [InlineData("Æ", "Ć")]
        [InlineData("Ê", "Ę")]
        [InlineData("£", "Ł")]
        [InlineData("Ñ", "Ń")]
        [InlineData("Œ", "Ś")]
        [InlineData("¯", "Ż")]
        [InlineData("\u008F", "Ź")]
        public void Translate_SingleSpecialCharacter_OutputCharacterTranslatedCorectly(string input, string output)
        {
            var result = _translateAnsiCharactersToPolishService.Translate(input);

            Assert.Equal(output, result);
        }
    }
}
