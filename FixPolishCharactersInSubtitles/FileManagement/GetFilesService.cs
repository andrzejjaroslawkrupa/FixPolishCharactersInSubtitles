using System.IO.Abstractions;
using System.Reflection;

namespace FixPolishCharactersInSubtitles.FileManagement
{
    public class GetFilesService : IGetFiles
    {
        private readonly IFileSystem _fileSystem;

        public GetFilesService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public List<string> GetLocalFiles()
        {
            string strExeFilePath = Assembly.GetExecutingAssembly().Location;

            if (strExeFilePath == null)
                throw new NullReferenceException($"Path to executable directory is null. Executable path: {strExeFilePath}");

            string? strWorkPath = _fileSystem.Path.GetDirectoryName(strExeFilePath);

            if (!_fileSystem.Directory.Exists(strWorkPath))
                throw new DirectoryNotFoundException();

            return GetFilesFromDir(strWorkPath);
        }

        public List<string> GetFilesFromDir(string directory)
        {
            List<string> files = new();
            try
            {
                files.AddRange(_fileSystem.Directory.GetFiles(directory));

                foreach (string dir in _fileSystem.Directory.GetDirectories(directory))
                {
                    files.AddRange(GetFilesFromDir(dir));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return files;
        }
    }
}
