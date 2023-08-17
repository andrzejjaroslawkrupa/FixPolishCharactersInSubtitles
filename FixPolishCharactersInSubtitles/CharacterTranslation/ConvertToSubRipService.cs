using FixPolishCharactersInSubtitles.Abstractions;
using FixPolishCharactersInSubtitles.CharacterTranslation.Converters;
using FixPolishCharactersInSubtitles.CharacterTranslation.Enums;
using System.IO.Abstractions;
using System.Text.RegularExpressions;

namespace FixPolishCharactersInSubtitles.CharacterTranslation
{
    public class ConvertToSubRipService : IConvertToSubRipService
    {
        private readonly IFileSystem _fileSystem;

        public ConvertToSubRipService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        private const string MicroDVDFileFormatRegex = @"^\{\d+\}\{\d+\}.+";

        public string ConvertContentToSubRip(string content)
        {
            SubtitleFormat inputFormat = DetermineSubtitleFormat(content);

            switch (inputFormat)
            {
                case SubtitleFormat.MicroDVD:
                    return MicroDVDConverter.ConvertFromMicroDVD(content);
                default:
                    throw new FormatException("Unknown subtitle format");
            }
        }

        private static SubtitleFormat DetermineSubtitleFormat(string inputContent)
        {
            if (Regex.IsMatch(inputContent, MicroDVDFileFormatRegex))
            {
                return SubtitleFormat.MicroDVD;
            }
            else
            {
                return SubtitleFormat.Unknown;
            }
        }

        public string ConvertPathToSrt(string path)
        {
            string? directory = _fileSystem.Path.GetDirectoryName(path);

            if (directory == null)
                throw new NullReferenceException(directory);

            if (!_fileSystem.Directory.Exists(directory))
                throw new DirectoryNotFoundException(path);

            string? fileNameWithoutExtension = _fileSystem.Path.GetFileNameWithoutExtension(path);

            if (fileNameWithoutExtension == null)
                throw new NullReferenceException(fileNameWithoutExtension);

            return _fileSystem.Path.Combine(directory, $"{fileNameWithoutExtension}.srt");
        }
    }
}
