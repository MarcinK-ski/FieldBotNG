namespace TunnelingTools.Settings
{
    /// <summary>
    /// Settings of created tunnel
    /// </summary>
    public class TunnelSettings
    {
        /// <summary>
        /// IP or Hostname
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Host port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// If it's local device, User and Password are useless
        /// </summary>
        public bool IsLocalDevice { get; set; }

        /// <summary>
        /// Host user
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Host password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets address IP and port number
        /// </summary>
        /// <returns>String in format `IP:Port`</returns>
        public string GetPortAndIP()
        {
            return $"{IP}:{Port}";
        }
    }
}
