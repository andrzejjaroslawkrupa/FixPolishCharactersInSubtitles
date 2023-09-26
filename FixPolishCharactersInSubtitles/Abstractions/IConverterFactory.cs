namespace FixPolishCharactersInSubtitles.Abstractions
{
    public interface IConverterFactory
    {
        IConverter CreateConverter(string inputContent);
    }
}
