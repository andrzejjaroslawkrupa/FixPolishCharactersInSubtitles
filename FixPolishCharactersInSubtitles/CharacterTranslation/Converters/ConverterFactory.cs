using FixPolishCharactersInSubtitles.Abstractions;
using System.Text.RegularExpressions;

namespace FixPolishCharactersInSubtitles.CharacterTranslation.Converters
{
    public class ConverterFactory : IConverterFactory
    {
        private const string MicroDVDFileFormatRegex = @"^\{\d+\}\{\d+\}.+";
        private const string CustomTimeStampFormatRegex = @"\[(\d+)\]\[(\d+)\](.*)";

        public IConverter CreateConverter(string inputContent)
        {
            if (Regex.IsMatch(inputContent, MicroDVDFileFormatRegex))
                return new MicroDVDConverter();

            if (Regex.IsMatch(inputContent, CustomTimeStampFormatRegex))
                return new CustomTimeStampConverter();

            throw new FormatException("Unknown subtitle format");
        }
    }
}
