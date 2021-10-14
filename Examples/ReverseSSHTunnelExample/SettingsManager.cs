using Microsoft.Extensions.Configuration;
using TunnelingTools;

namespace ReverseSSHTunnelExample
{
    /// <summary>
    /// Provide methods to get data from config file
    /// </summary>
    public static class SettingsManager
    {
        /// <summary>
        /// File name or location with name
        /// </summary>
        public const string APP_SETTINGS_JSON_LOCATION = "appsettings.json";
        /// <summary>
        /// In JSON - Name of root
        /// </summary>
        public const string ROOT_OBJECT_NAME_IN_JSON = "appsettings";

        private static ApplicationSettings _appConfig;
        public static ApplicationSettings AppConfig
        {
            get
            {
                if (_appConfig == null)
                {
                    IConfigurationRoot configurationRoot = new ConfigurationBuilder()
                                                               .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                                                               .AddJsonFile(APP_SETTINGS_JSON_LOCATION)
                                                               .Build();

                    _appConfig = configurationRoot.GetSection(ROOT_OBJECT_NAME_IN_JSON).Get<ApplicationSettings>();
                }

                return _appConfig;
            }
        }
    }

    /// <summary>
    /// Application settings root class
    /// </summary>
    public class ApplicationSettings
    {
        /// <summary>
        /// Is BashTools works on WSL or native LinuxOS
        /// </summary>
        public bool WSL { get; set; }

        /// <summary>
        /// Avaliable hosts
        /// </summary>
        public EndToEndHosts[] Hosts { get; set; }
    }

    /// <summary>
    /// Class with details for: in-LAN-host and remote host
    /// </summary>
    public class EndToEndHosts
    {
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
