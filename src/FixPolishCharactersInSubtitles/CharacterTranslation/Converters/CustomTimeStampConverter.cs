using FixPolishCharactersInSubtitles.Abstractions;
using System.Text;
using System.Text.RegularExpressions;

namespace FixPolishCharactersInSubtitles.CharacterTranslation.Converters
{
    internal class CustomTimeStampConverter : IConverter
    {
        private const string CustomTimeStampFormatRegex = @"\[(\d+)\]\[(\d+)\](.+)";

        public string ConvertToSubRip(string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
                throw new ArgumentNullException(nameof(inputText));

            string[] subtitleEntries = inputText.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

            StringBuilder srtBuilder = new();
            int subtitleNumber = 1;

            foreach (string entry in subtitleEntries)
            {
                Match match = Regex.Match(entry, CustomTimeStampFormatRegex, RegexOptions.Singleline);

                if (match.Success)
                {
                    int startTimeInMiliseconds = int.Parse(match.Groups[1].Value) * 100;
                    int endTimeInMiliseconds = int.Parse(match.Groups[2].Value) * 100;
                    string subtitleText = match.Groups[3].Value.Replace("|", Environment.NewLine).Trim();

                    srtBuilder.AppendLine(subtitleNumber.ToString());
                    srtBuilder.AppendLine($"{ConvertMillisecondsToTimeFormat(startTimeInMiliseconds)} --> {ConvertMillisecondsToTimeFormat(endTimeInMiliseconds)}");
                    srtBuilder.AppendLine(subtitleText);
                    srtBuilder.AppendLine(); // Blank line between entries

                    subtitleNumber++;
                }
            }

            return srtBuilder.ToString().Trim();
        }

        private static string ConvertMillisecondsToTimeFormat(int milliseconds)
        {
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(milliseconds);

            return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2},{timeSpan.Milliseconds:D3}";
        }
    }
}
