namespace BashTools.Settings
{
    /// <summary>
    /// Application settings root class
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// Is BashTools works on WSL or native LinuxOS
        /// </summary>
        public bool WSL { get; set; }
    }
}
