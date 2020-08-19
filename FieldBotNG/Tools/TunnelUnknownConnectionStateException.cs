using System;

namespace FieldBotNG.Tools
{
    internal class TunnelUnknownConnectionStateException : Exception
    {
        public TunnelUnknownConnectionStateException(string message) : base(message)
        {
        }
    }
}