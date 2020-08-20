using System;

namespace TunnelingTools
{
    /// <summary>
    /// Represents error that occurs while trying to estabilish tunnel while other connection is already estabilished.
    /// </summary>
    class TunnelEstablishedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of TunnelEstablishedException
        /// </summary>
        /// <param name="message">Cause of exception</param>
        public TunnelEstablishedException(string message) : base(message)
        {
        }
    }
}