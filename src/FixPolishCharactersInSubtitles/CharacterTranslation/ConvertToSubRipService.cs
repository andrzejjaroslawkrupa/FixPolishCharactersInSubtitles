using FixPolishCharactersInSubtitles.Abstractions;
using System.IO.Abstractions;

namespace FixPolishCharactersInSubtitles.CharacterTranslation
{
    public class ConvertToSubRipService : IConvertToSubRipService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IConverterFactory _converterFactory;

        public ConvertToSubRipService(IFileSystem fileSystem, IConverterFactory converterFactory)
        {
            _fileSystem = fileSystem;
            _converterFactory = converterFactory;
        }

        public string ConvertContentToSubRip(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));

            var converter = _converterFactory.CreateConverter(content);
            return converter.ConvertToSubRip(content);
        }

        public string ConvertPathToSrt(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            string? directory = _fileSystem.Path.GetDirectoryName(path);

            if (directory == null)
                throw new NullReferenceException(nameof(directory));

            if (!_fileSystem.Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            string? fileNameWithoutExtension = _fileSystem.Path.GetFileNameWithoutExtension(path);

            if (fileNameWithoutExtension == null)
                throw new NullReferenceException(nameof(fileNameWithoutExtension));

            return _fileSystem.Path.Combine(directory, $"{fileNameWithoutExtension}.srt");
        }
    }
}
