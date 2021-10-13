using System.Text.RegularExpressions;

namespace FieldBotNG.Templates
{
    static class Commands
    {
        public const char PREFIX = '!';   // Allowed only non-letter and non-whitespace chars
        public const string PATTERN_FOR_NON_SUFFIX = @"^\{0}\s?{1}\s?$";
        public const string PATTERN_FOR_DIGITS_SUFFIX = @"^\{0}\s?{1}\s?\d+\s?$";

        public const string HELP = "pomoc";
        public const string CONNECT = "p";
        public const string DISCONNECT = "r";
        public const string CONNECTION_STATE = "s";
        public const string ALL_ACTIVE_CONNECTIONS = "w";
        public const string ALL_AVALIABLE_CONNECTIONS = "u";

        public static bool IsCommandMatchPattern(this string receivedMessage, string command, bool isDigitSuffix)
        {
            string prePattern = isDigitSuffix ? PATTERN_FOR_DIGITS_SUFFIX : PATTERN_FOR_NON_SUFFIX;
            string pattern = string.Format(prePattern, PREFIX, command);

            return Regex.IsMatch(receivedMessage, pattern);
        }
    }
}
