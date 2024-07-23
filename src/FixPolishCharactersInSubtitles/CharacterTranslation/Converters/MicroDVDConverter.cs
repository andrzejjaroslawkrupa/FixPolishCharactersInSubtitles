using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FixPolishCharactersInSubtitles.Abstractions;

namespace FixPolishCharactersInSubtitles.CharacterTranslation.Converters
{
    internal class MicroDVDConverter : IConverter
    {
        private const string MicroDVDFormatRegex = @"\{(\d+)\}\{(\d+)\}(.+)";
        private const int DefaultFPSValue = 24;

        public string ConvertToSubRip(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content));

            string[] lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            double framesPerSecond = GetFramesPerSecond(lines[0]);

            int sequenceNumber = 1;
            StringBuilder srtContent = new StringBuilder();
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (!string.IsNullOrEmpty(trimmedLine))
                {
                    Match match = Regex.Match(trimmedLine, MicroDVDFormatRegex);
                    if (match.Success)
                    {
                        double startFrame = int.Parse(match.Groups[1].Value);
                        double endFrame = int.Parse(match.Groups[2].Value);
                        string subtitleText = match.Groups[3].Value.Replace("|", Environment.NewLine);

                        TimeSpan startTime = TimeSpan.FromSeconds(startFrame / framesPerSecond);
                        TimeSpan endTime = TimeSpan.FromSeconds(endFrame / framesPerSecond);

                        string startTimeFormatted = FormatTimestamp(startTime);
                        string endTimeFormatted = FormatTimestamp(endTime);

                        srtContent.AppendLine(sequenceNumber.ToString());
                        srtContent.AppendLine($"{startTimeFormatted} --> {endTimeFormatted}");
                        srtContent.AppendLine(subtitleText);
                        srtContent.AppendLine(); // Add an empty line between entries

                        sequenceNumber++;
                    }
                }
            }
            return srtContent.ToString();
        }

        private static double GetFramesPerSecond(string line)
        {
            Match match = Regex.Match(line, MicroDVDFormatRegex);

            if (match.Success && double.TryParse(match.Groups[3].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var fps) && IsNumberInRasonableFPSRange(fps))
                return fps;

            return DefaultFPSValue;
        }

        private static bool IsNumberInRasonableFPSRange(double number)
        {
            return number > 10 && number < 300;
        }

        private static string FormatTimestamp(TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2},{timeSpan.Milliseconds:D3}";
        }
    }
}
