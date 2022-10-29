using System.IO.Abstractions;
using System.Reflection;

namespace FixPolishCharactersInSubtitles.FileManagement
{
    public class GetLocalFilesService : IGetLocalFiles
    {
        private readonly IFileSystem _fileSystem;

        public GetLocalFilesService(IFileSystem fileSystem)
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

            return GetFilesFromPath(strWorkPath);
        }

        private List<string> GetFilesFromPath(string path)
        {
            List<string> files = new();
            try
            {
                files.AddRange(_fileSystem.Directory.GetFiles(path));

                foreach (string dir in _fileSystem.Directory.GetDirectories(path))
                {
                    files.AddRange(GetFilesFromPath(dir));
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
