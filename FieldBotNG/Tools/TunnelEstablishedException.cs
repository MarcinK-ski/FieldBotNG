using System;

namespace FieldBotNG.Tools
{
    class TunnelEstablishedException : Exception
    {
        public TunnelEstablishedException(string message) : base(message)
        {
        }
    }
}