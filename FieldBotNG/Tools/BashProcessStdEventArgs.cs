using System;

namespace FieldBotNG.Tools
{
    public class BashProcessStdEventArgs : EventArgs
    {
        /// <summary>
        /// Gets data from StdOutput
        /// </summary>
        public string Output { get; }

        /// <summary>
        /// Gets data from StdError
        /// </summary>
        public string Error { get; }

        /// <summary>
        /// Provides data from BashProcess if data on StandardOutput or StandardError occured.
        /// </summary>
        /// <param name="stdString">String on Standard output or error</param>
        /// <param name="isStdError">Define is string StandardOutput or StandardError</param>
        public BashProcessStdEventArgs(string stdString, bool isStdError)
        {
            if (isStdError)
            {
                Error = stdString;
            }
            else
            {
                Output = stdString;
            }
        }
    }
}