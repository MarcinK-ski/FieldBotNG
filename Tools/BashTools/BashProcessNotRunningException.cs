using System;

namespace BashTools
{
    /// <summary>
    /// Represents error that occur when BashProcess is not running and there was try to do something forbidden in this state
    /// </summary>
    public class BashProcessNotRunningException : Exception
    {
        /// <summary>
        /// Initializes a new instance of BashProcessNotRunningException
        /// </summary>
        public BashProcessNotRunningException()
        {
        }
    }
}