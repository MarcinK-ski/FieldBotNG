using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FieldBotNG.Settings
{
    public class ApplicationSettings
    {
        public DiscordBotSettings DiscordBot { get; set; }

        public TunnelSettings RemoteHost { get; set; }

        public TunnelSettings LocalHost { get; set; }

        public bool WSL { get; set; }
    }
}
