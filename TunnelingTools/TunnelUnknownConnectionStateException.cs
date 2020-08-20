using System;

namespace TunnelingTools
{
    internal class TunnelUnknownConnectionStateException : Exception
    {
        public TunnelUnknownConnectionStateException(string message) : base(message)
        {
        }
    }
}