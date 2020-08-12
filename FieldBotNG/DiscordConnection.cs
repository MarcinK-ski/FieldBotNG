using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FieldBotNG
{
    class DiscordConnection
    {
        private static DiscordSocketClient _client;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _client.Log += Log;

            await _client.LoginAsync(Discord.TokenType.Bot, Helper.AppConfig.DiscordBot.Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private static Task Log(Discord.LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
    }
}
