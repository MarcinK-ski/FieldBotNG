namespace TunnelingTools.Settings
{
    public class ApplicationSettings : BashTools.Settings.ApplicationSettings
    {
        public TunnelSettings RemoteHost { get; set; }

        public TunnelSettings LocalHost { get; set; }
    }
}
