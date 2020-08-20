using Newtonsoft.Json;
using System.Collections.Generic;

namespace FieldBotNG.Settings
{
    public class DiscordBotSettings
    {
        /// <summary>
        /// Bot name (optional)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Bot token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// List of allowed channels IDs 
        /// </summary>
        public List<ulong> AllowedChannels { get; set; }
    }
}
