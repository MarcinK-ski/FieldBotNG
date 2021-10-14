namespace TunnelingTools
{
    /// <summary>
    /// Class with host fields
    /// </summary>
    public class Host
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
        /// Gets address IP and port number
        /// </summary>
        /// <returns>String in format `IP:Port`</returns>
        public string GetPortAndIP()
        {
            return $"{IP}:{Port}";
        }
    }
}
