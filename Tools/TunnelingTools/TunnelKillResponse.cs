namespace TunnelingTools
{
    /// <summary>
    /// Contains fields with details after destroying tunnel
    /// </summary>
    public class TunnelDestroyResponse
    {
        /// <summary>
        /// Contains current tunnel connection state
        /// </summary>
        public TunnelConnectionState TunnelConnectionState { get; set; }

        /// <summary>
        /// Contains exception message if occurs
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// Creates instance with details about destroying tunnel
        /// </summary>
        /// <param name="connectionState">Connection state after tunnel destroy</param>
        /// <param name="exceptionMessage">Exception message. If exception wasn't throw, leave as null</param>
        public TunnelDestroyResponse(TunnelConnectionState connectionState, string exceptionMessage = null)
        {
            TunnelConnectionState = connectionState;
            ExceptionMessage = exceptionMessage;
        }
    }
}
