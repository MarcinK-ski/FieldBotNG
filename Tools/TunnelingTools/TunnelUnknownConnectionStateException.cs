using System;

namespace TunnelingTools
{
    /// <summary>
    /// Represents error that occurs when tunnel connection state is unknown.
    /// (ex. netstat returns empty string or local address returns address other than 0.0.0.0/127.0.0.1)
    /// </summary>
    public class TunnelUnknownConnectionStateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of TunnelUnknownConnectionStateException
        /// </summary>
        /// <param name="message">Cause of exception</param>
        public TunnelUnknownConnectionStateException(string message) : base(message)
        {
        }
    }
}