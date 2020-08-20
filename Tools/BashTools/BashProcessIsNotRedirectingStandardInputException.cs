using System;

namespace BashTools
{
    /// <summary>
    /// Represents error that occur when StandardInput redirect is false.
    /// </summary>
    public class BashProcessIsNotRedirectingStandardInputException : Exception
    {
        /// <summary>
        /// Initializes a new instance of BashProcessIsNotRedirectingStandardInputException
        /// </summary>
        public BashProcessIsNotRedirectingStandardInputException()
        {
        }
    }
}