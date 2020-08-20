namespace TunnelingTools.Settings
{
    /// <summary>
    /// Application settings root class
    /// </summary>
    public class ApplicationSettings : BashTools.Settings.ApplicationSettings
    {
        /// <summary>
        /// Remote host details
        /// </summary>
        public TunnelSettings RemoteHost { get; set; }

        /// <summary>
        /// Local side host details
        /// </summary>
        public TunnelSettings LocalHost { get; set; }
    }
}
