using FieldBotNG.Settings;
using Microsoft.Extensions.Configuration;

namespace FieldBotNG
{
    public static class SettingsManager
    {
        public const string APP_SETTINGS_JSON_LOCATION = "appsettings.json";
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
}
