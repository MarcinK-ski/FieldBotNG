using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace FieldBotNG.Settings
{
    class ApplicationSettings
    {
        public DiscordBotSettings DiscordBot { get; set; }

        public TunnelSettings Tunnel { get; set; }
    }
}
