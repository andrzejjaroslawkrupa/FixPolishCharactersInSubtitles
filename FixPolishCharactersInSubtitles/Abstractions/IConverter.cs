namespace FixPolishCharactersInSubtitles.Abstractions
{
    public interface IConverter
    {
        string ConvertToSubRip(string content);
    }
}