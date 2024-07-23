using FixPolishCharactersInSubtitles.Abstractions;

namespace FixPolishCharactersInSubtitles
{
    public class CommandLineInterface : ICommandLineInterface
    {
        public string[] GetCommandLineArgs()
        {
            return Environment.GetCommandLineArgs();
        }
    }
}
