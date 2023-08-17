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
        private readonly IConvertToSubRipService _convertToSrtService;
        private readonly IFileSystem _fileSystem;

        public FixPolishCharactersInAllFilesInCurrentDirectory(
            IGetLocalFiles localFilesManager,
            ITranslateCharactersService translateCharactersService,
            IConvertToSubRipService convertToSrtService,
            IFileSystem fileSystem)
        {
            _localFilesManager = localFilesManager;
            _translateCharactersService = translateCharactersService;
            _convertToSrtService = convertToSrtService;
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
            ".sub",
            ".txt"
        };

        private void ConvertFile(string path)
        {
            string subsText = _fileSystem.File.ReadAllText(path, Encoding.GetEncoding(1252));
            string replaced = _translateCharactersService.Translate(subsText);
            if (Path.GetExtension(path) != ".srt")
            {
                replaced = _convertToSrtService.ConvertContentToSubRip(replaced);
                path = _convertToSrtService.ConvertPathToSrt(path);
            }
            _fileSystem.File.WriteAllText(path, replaced, Encoding.UTF8);
        }
    }
}
