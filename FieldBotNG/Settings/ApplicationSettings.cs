namespace FieldBotNG.Settings
{
    /// <summary>
    /// Application settings root class
    /// </summary>
    public class ApplicationSettings : TunnelingTools.Settings.ApplicationSettings
    {
        /// <summary>
        /// Discord bot details
        /// </summary>
        public DiscordBotSettings DiscordBot { get; set; }
    }
}
