using System;

namespace TunnelingTools
{
    class TunnelEstablishedException : Exception
    {
        public TunnelEstablishedException(string message) : base(message)
        {
        }
    }
}