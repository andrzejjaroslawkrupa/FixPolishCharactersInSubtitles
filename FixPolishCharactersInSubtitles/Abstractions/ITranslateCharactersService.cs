using System.Text;

namespace FixPolishCharactersInSubtitles.Abstractions
{
    public interface ITranslateCharactersService
    {
        string Translate(string text);
    }
}
