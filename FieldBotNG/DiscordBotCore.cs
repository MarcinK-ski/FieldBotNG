using Discord.WebSocket;
using System;
using System.Text;
using System.Threading.Tasks;
using TunnelingTools;

namespace FieldBotNG
{
    public class DiscordBotCore
    {
        private static DiscordSocketClient _client;

        private ReverseSSHTunnel _tunnel;

        public async Task MainAsync()
        {
            BashTools.BashProcess.IsWSL = true;

            _tunnel = new ReverseSSHTunnel(SettingsManager.AppConfig.RemoteHost, SettingsManager.AppConfig.LocalHost);

            _client = new DiscordSocketClient();
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;

            await _client.LoginAsync(Discord.TokenType.Bot, SettingsManager.AppConfig.DiscordBot.Token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task MessageReceived(SocketMessage message)
        {
            if (message.Content.StartsWith('!'))
            {
                if (SettingsManager.AppConfig.DiscordBot.AllowedChannels.Contains(message.Channel.Id))
                {
                    await HandleMessage(message);
                }
            }
        }

        private async Task HandleMessage(SocketMessage message)
        {
            string content = message.Content.ToLower();

            if (content.Contains("pomoc")) // Help
            {
                StringBuilder helpContent = new StringBuilder();

                helpContent.Append("Każda komenda musi zaczynać się znakiem: `!` *(wykrzyknik)*");
                helpContent.Append($"\nLista komend: ");
                helpContent.Append($"\n    - `p` - otwiera połączenie z rejestratorem, ");
                helpContent.Append($"\n    - `r` - zamyka połączenie z rejestratorem,");
                helpContent.Append($"\n    - `s` - sprawdza, czy połączenie z rejestratorem jest aktywne,");
                helpContent.Append($"\n    - `pomoc` - wyświetla pomoc, czyli tę listę komend.");

                await message.Channel.SendMessageAsync(helpContent.ToString());
            }
            else if (content.Contains('p'))  // Connect - "Polacz"
            {
                if (_tunnel.IsTunnelEstablished)
                {
                    await message.Channel.SendMessageAsync("Tunel już istnieje. Stan możesz sprawdzić wpisując `!s`. Jeśli po wpisaniu `!s`, pojawi się informacja o braku połączenia, spróbuj jeszcze raz.");
                }
                else
                {
                    bool isTunnelStarted = _tunnel.Start();

                    if (isTunnelStarted)
                    {
                        await message.Channel.SendMessageAsync("Połączenie zostało utworzone!");
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync("Wystąpił **problem** z utworzeniem połączenia zdalnego!");
                    }
                }
            }
            else if (content.Contains('r')) // Disconnect - "Rozlacz"
            {
                TunnelConnectionState tunnelConnectionStateAfterStopped = await _tunnel.Stop();
                if (tunnelConnectionStateAfterStopped == TunnelConnectionState.NoConnection)
                {
                    await message.Channel.SendMessageAsync("Pomyślnie rozłączono!");
                }
                else
                {
                    await message.Channel.SendMessageAsync($"Próbowano rozłączyć, ale wystąpił błąd. Aktualny stan to: *{tunnelConnectionStateAfterStopped}*. **Spradź koniecznie stan za jakiś czas!**");
                }
            }
            else if (content.Contains('s')) // Connection status - "Status polaczenia"
            {
                TunnelConnectionState tunnelConnectionState = await _tunnel.CheckConnectionType();
                await message.Channel.SendMessageAsync($"Aktualny status połączenia, to: *{tunnelConnectionState}*");
            }
        }

        private static Task Log(Discord.LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
    }
}
