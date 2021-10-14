using System.Text.RegularExpressions;

namespace FieldBotNG.Templates
{
    static class Commands
    {
        public const char PREFIX = '!';   // Allowed only non-letter and non-whitespace chars
        public const string PATTERN_FOR_NON_SUFFIX = @"^\{0}\s?{1}\s?$";
        public const string PATTERN_FOR_DIGITS_SUFFIX = @"^\{0}\s?{1}\s?(\d+)\s?$";

        public const string HELP = "pomoc";
        public const string CONNECT = "p";
        public const string DISCONNECT = "r";
        public const string CONNECTION_STATE = "s";
        public const string ALL_ACTIVE_CONNECTIONS = "w";
        public const string ALL_AVALIABLE_CONNECTIONS = "u";

        public const string KILL_YOURSELF = "^kys";

        public static bool IsCommandMatchPattern(this string receivedMessage, string command, bool isDigitSuffix)
        {
            return IsCommandMatchPattern(receivedMessage, command, isDigitSuffix, out int? _);
        }

        public static bool IsCommandMatchPattern(this string receivedMessage, string command, bool isDigitSuffix, out int? foundNumber)
        {
            bool isMatch = false;
            foundNumber = null;

            string prePattern = isDigitSuffix ? PATTERN_FOR_DIGITS_SUFFIX : PATTERN_FOR_NON_SUFFIX;
            string pattern = string.Format(prePattern, PREFIX, command);

            if (isDigitSuffix)
            {
                Match regexMatch = Regex.Match(receivedMessage, pattern);

                if (regexMatch.Success)
                {
                    isMatch = true;

                    if (regexMatch.Groups.Count > 1)
                    {
                        bool intParsed = int.TryParse(regexMatch.Groups[1].Value, out int parsedNumber);
                        if (intParsed)
                        {
                            foundNumber = parsedNumber;
                        }
                        else
                        {
                            System.Console.WriteLine("Int parsing value from first group failed!");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Command is OK, but there's no group with number found.");
                    }
                }
            }
            else
            {
                isMatch = Regex.IsMatch(receivedMessage, pattern);
            }

            return isMatch;
        }
    }
}
