namespace FixPolishCharactersInSubtitles.FileManagement
{
    public interface IGetFiles
    {
        List<string> GetLocalFiles();
        List<string> GetFilesFromDir(string directory);
    }
}
