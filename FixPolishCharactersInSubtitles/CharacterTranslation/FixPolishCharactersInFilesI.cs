using FixPolishCharactersInSubtitles.Abstractions;
using FixPolishCharactersInSubtitles.FileManagement;
using System.IO.Abstractions;
using System.Text;

namespace FixPolishCharactersInSubtitles.CharacterTranslation
{
    public class FixPolishCharactersInFilesI : IConvertCharactersInFiles
    {
        private readonly IGetFiles _localFilesManager;
        private readonly ITranslateCharactersService _translateCharactersService;
        private readonly IConvertToSubRipService _convertToSrtService;
        private readonly IFileSystem _fileSystem;
        private readonly ICommandLineInterface _commandLineInterface;

        public FixPolishCharactersInFilesI(
            IGetFiles localFilesManager,
            ITranslateCharactersService translateCharactersService,
            IConvertToSubRipService convertToSrtService,
            IFileSystem fileSystem,
            ICommandLineInterface commandLineInterface)
        {
            _localFilesManager = localFilesManager;
            _translateCharactersService = translateCharactersService;
            _convertToSrtService = convertToSrtService;
            _fileSystem = fileSystem;
            _commandLineInterface = commandLineInterface;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public void Convert()
        {
            var arguments = _commandLineInterface.GetCommandLineArgs();

            var index = Array.IndexOf(arguments, "--path");
            if (index >= 0)
            {
                index++;
                var path = arguments[index];
                if (!string.IsNullOrEmpty(path))
                    ConvertAllFiles(path);
                else
                    throw new ArgumentNullException(nameof(path));
            }
            else
                ConvertAllLocalFiles();
        }

        private void ConvertAllFiles(string directory)
        {
            if (!_fileSystem.Directory.Exists(directory))
                throw new DirectoryNotFoundException(directory);

            var allFiles = _localFilesManager.GetFilesFromDir(directory);
            ConvertFilesFromList(allFiles);
        }

        private void ConvertAllLocalFiles()
        {
            var allFiles = _localFilesManager.GetLocalFiles();
            ConvertFilesFromList(allFiles);
        }

        private void ConvertFilesFromList(List<string> allFiles)
        {
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
                try
                {
                    replaced = _convertToSrtService.ConvertContentToSubRip(replaced);
                    path = _convertToSrtService.ConvertPathToSrt(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Convertion to SubRip has failed: {e}");
                }
            }
            _fileSystem.File.WriteAllText(path, replaced, Encoding.UTF8);
        }
    }
}
