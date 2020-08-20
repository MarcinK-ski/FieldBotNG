namespace TunnelingTools.Settings
{
    public class TunnelSettings
    {
        public string IP { get; set; }

        public int Port { get; set; }

        /// <summary>
        /// If it's local device, User and Password are useless
        /// </summary>
        public bool IsLocalDevice { get; set; }

        public string User { get; set; }

        public string Password { get; set; }
    }
}
