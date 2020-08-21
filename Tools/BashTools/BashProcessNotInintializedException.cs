using System;

namespace BashTools
{
    /// <summary>
    /// Represents error that occur when BashProcess is not initialized
    /// </summary>
    public class BashProcessNotInintializedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of BashProcessNotInintializedException
        /// </summary>
        public BashProcessNotInintializedException()
        {
        }
    }
}