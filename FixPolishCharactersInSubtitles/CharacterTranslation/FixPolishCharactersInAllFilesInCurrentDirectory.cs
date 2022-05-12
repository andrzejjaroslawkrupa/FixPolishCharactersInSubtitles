using FixPolishCharactersInSubtitles.Abstractions;
using FixPolishCharactersInSubtitles.FileManagement;
using System.IO.Abstractions;
using System.Text;

namespace FixPolishCharactersInSubtitles.CharacterTranslation
{
    public class FixPolishCharactersInAllFilesInCurrentDirectory : IConvertCharactersInAllFiles
    {
        private readonly IGetLocalFiles _localFilesManager;
        private readonly ITranslateCharactersService _translateCharactersService;
        private readonly IFileSystem _fileSystem;

        public FixPolishCharactersInAllFilesInCurrentDirectory(
            IGetLocalFiles localFilesManager,
            ITranslateCharactersService translateCharactersService,
            IFileSystem fileSystem)
        {
            _localFilesManager = localFilesManager;
            _translateCharactersService = translateCharactersService;
            _fileSystem = fileSystem;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public void ConvertAllFiles()
        {
            var allFiles = _localFilesManager.GetLocalFiles();

            foreach (var subs in allFiles.Where(f => _supportedFileExtensions.Any(s => f.EndsWith(s))))
            {
                ConvertFile(subs);
            }
        }

        private readonly List<string> _supportedFileExtensions = new()
        {
            ".srt",
            ".txt"
        };

        private void ConvertFile(string path)
        {
            string subsText = _fileSystem.File.ReadAllText(path, Encoding.GetEncoding(1252));
            string replaced = _translateCharactersService.Translate(subsText);
            File.WriteAllText(path, replaced, Encoding.UTF8);
        }
    }
}
