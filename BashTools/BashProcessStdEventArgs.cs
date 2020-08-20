using System;

namespace BashTools
{
    public class BashProcessStdEventArgs : EventArgs
    {
        /// <summary>
        /// Gets data from StdOutput/StdError
        /// </summary>
        public string Output { get; }

        /// <summary>
        /// Provides data from BashProcess if data on StandardOutput or StandardError occured.
        /// </summary>
        /// <param name="stdString">String on Standard output or error</param>
        public BashProcessStdEventArgs(string stdString)
        {
            Output = stdString;
        }
    }
}