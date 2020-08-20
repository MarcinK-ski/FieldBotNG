namespace TunnelingTools
{
    /// <summary>
    /// Possible states of tunnel connection
    /// </summary>
    public enum TunnelConnectionState
    {
        /// <summary>
        /// You can connect to port, using public address.
        /// </summary>
        RemoteConnection,

        /// <summary>
        /// It's not allowed to connect via public address - when netstat shows Local Address as `127.0.0.1:port`. 
        /// (Tip: Set `GatewayPorts` value as `clientspecified` and type bind_address as one of following: 
        /// `0.0.0.0:port:...` or `\*:port:...` or `"[::]:port:..."`. You can only set `GatewayPorts` value as `yes`, but it's not recomended)
        /// </summary>
        LocalConnection,

        /// <summary>
        /// There no tunnel connection
        /// </summary>
        NoConnection
    }
}