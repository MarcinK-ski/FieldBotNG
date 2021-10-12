using System.Collections.Generic;
using TunnelingTools;

namespace FieldBotNG.Settings
{
    /// <summary>
    /// Application settings root class
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// Discord bot details
        /// </summary>
        public DiscordBotSettings DiscordBot { get; set; }

        /// <summary>
        /// Avaliable hosts
        /// </summary>
        public EndToEndHosts[] Hosts { get; set; }

        /// <summary>
        /// Is BashTools works on WSL or native LinuxOS
        /// </summary>
        public bool WSL { get; set; }
    }

    /// <summary>
    /// Class with details for: in-LAN-host and remote host
    /// </summary>
    public class EndToEndHosts
    {
        /// <summary>
        /// Is this hosts pair set as default
        /// </summary>
        /// <remarks>
        /// Used in command service (to simplify creating connection)
        /// </remarks>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Is RemoteHost/RemotePort displaying as "Anonymous connection" in discord bot Activity
        /// </summary>
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// List of allowed discord users (as IDs)
        /// </summary>
        public List<ulong> AllowedUsers { get; set; }

        /// <summary>
        /// Remote host details
        /// </summary>
        public RemoteHost RemoteHost { get; set; }

        /// <summary>
        /// Local side host details
        /// </summary>
        public LocalHost LocalHost { get; set; }
    }
}
