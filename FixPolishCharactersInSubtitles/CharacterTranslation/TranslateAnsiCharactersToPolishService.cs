using FixPolishCharactersInSubtitles.Abstractions;

namespace FixPolishCharactersInSubtitles.CharacterTranslation
{
    public class TranslateAnsiCharactersToPolishService : ITranslateCharactersService
    {
        public string Translate(string text)
        {
            if (text == null)
                throw new ArgumentNullException("Provided text is null");

            foreach (var character in _characters)
            {
                text = text.Replace(character.Key, character.Value);
            }
            return text;
        }

        private readonly Dictionary<char, char> _characters = new Dictionary<char, char>()
        {
            { '¹', 'ą' },
            { 'æ', 'ć' },
            { 'ê', 'ę' },
            { '³', 'ł' },
            { 'ñ', 'ń' },
            { 'œ', 'ś' },
            { '¿', 'ż' },
            { 'Ÿ', 'ź' },
            { '¥', 'Ą' },
            { 'Æ', 'Ć' },
            { 'Ê', 'Ę' },
            { '£', 'Ł' },
            { 'Ñ', 'Ń' },
            { 'Œ', 'Ś' },
            { '¯', 'Ż' },
        };
    }
}
