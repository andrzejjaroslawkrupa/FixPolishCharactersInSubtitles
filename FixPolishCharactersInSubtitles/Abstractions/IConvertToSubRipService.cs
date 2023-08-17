namespace FixPolishCharactersInSubtitles.Abstractions
{
    public interface IConvertToSubRipService
    {
        string ConvertPathToSrt(string path);
        string ConvertContentToSubRip(string content);
    }
}