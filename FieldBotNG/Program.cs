using BashTools;
using Discord;
using Discord.WebSocket;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TunnelingTools;

namespace FieldBotNG
{
    class Program
    {
        /// <summary>
        /// Discord client object
        /// </summary>
        private static DiscordSocketClient _client;

        /// <summary>
        /// Reverse SSH tunnel object
        /// </summary>
        private static ReverseSSHTunnel _tunnel;

        static async Task Main()
        {
            DisplayHelloInfo();

            BashProcess.IsWSL = SettingsManager.AppConfig.WSL;

            _tunnel = new ReverseSSHTunnel(SettingsManager.AppConfig.RemoteHost, SettingsManager.AppConfig.LocalHost);

            (TunnelConnectionState, string) tunnelConnectionState = await _tunnel.CheckAndKillOldProcesses();
            Console.WriteLine($"{DateTime.Now} -> Current tunnel state is: {tunnelConnectionState.Item1}");
            
            if (!string.IsNullOrWhiteSpace(tunnelConnectionState.Item2))
            {
                Console.WriteLine($"{DateTime.Now} -> Error while killing processes: {tunnelConnectionState.Item2}");
            }

            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;

            await _client.LoginAsync(TokenType.Bot, SettingsManager.AppConfig.DiscordBot.Token);
            await _client.StartAsync();
            await SetCurrentActivity(tunnelConnectionState.Item1);

            await Task.Delay(-1);
        }

        private static void DisplayHelloInfo()
        {
            string version = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            if (string.IsNullOrEmpty(version))
            {
                version = @"ver. N/A";
            }

            string helloMessageLeftRightSeparators = "*********";
            string helloMessageContent = $"THIS IS: FieldBotNG ({version}) Bot for Discord!";

            StringBuilder innerSeparatorBuilder = new StringBuilder();
            innerSeparatorBuilder.Append(helloMessageLeftRightSeparators);
            innerSeparatorBuilder.Append(new string(' ', helloMessageContent.Length + 2));
            innerSeparatorBuilder.Append(helloMessageLeftRightSeparators);

            string helloMessage = $"{helloMessageLeftRightSeparators} {helloMessageContent} {helloMessageLeftRightSeparators}";

            string edgeSeparatorsString = new string('*', helloMessage.Length);

            Console.WriteLine($"\n{edgeSeparatorsString} \n{edgeSeparatorsString} \n{innerSeparatorBuilder} \n{helloMessage} \n{innerSeparatorBuilder} \n{edgeSeparatorsString} \n{edgeSeparatorsString}");
        }

        /// <summary>
        /// Catching receiver messages.
        /// </summary>
        /// <param name="message">Received socket message object</param>
        /// <returns></returns>
        private static async Task MessageReceived(SocketMessage message)
        {
            if (message.Content.StartsWith('!'))
            {
                if (SettingsManager.AppConfig.DiscordBot.AllowedChannels.Contains(message.Channel.Id))
                {
                    await HandleMessage(message);
                }
            }
        }

        /// <summary>
        /// Handles received socket message object
        /// </summary>
        /// <param name="message">Received socket message object</param>
        /// <returns></returns>
        private static async Task HandleMessage(SocketMessage message)
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
                    await message.Channel.SendMessageAsync("Tunel już istnieje. Stan możesz sprawdzić wpisując `!s`. \nJeśli po wpisaniu `!s`, pojawi się informacja o braku połączenia, spróbuj jeszcze raz.");
                }
                else
                {
                    await message.Channel.SendMessageAsync("Próba utworzenia tunelu...");

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
                await message.Channel.SendMessageAsync("Czekaj...");

                TunnelConnectionState? tunnelConnectionState = await CheckConnection();

                if (_tunnel.IsTunnelEstablished)
                {
                    try
                    {
                        (TunnelConnectionState, string) stoppingResult = await _tunnel.Stop();
                        tunnelConnectionState = stoppingResult.Item1;

                        if (tunnelConnectionState == TunnelConnectionState.NoConnection)
                        {
                            await message.Channel.SendMessageAsync("Pomyślnie rozłączono!");
                        }
                        else if (tunnelConnectionState == TunnelConnectionState.StoppedWithoutChecking)
                        {
                            await message.Channel.SendMessageAsync($"Rozłączono, ale nie wystąpiło sprawdzenie istniejących połączeń. Treść błędu: {stoppingResult.Item2}");
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync($"Próbowano rozłączyć, ale wystąpił błąd. Aktualny stan to: *{tunnelConnectionState}*. \n**Spradź koniecznie stan za jakiś czas!**");
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex is BashProcessNotInintializedException)
                        {
                            await message.Channel.SendMessageAsync($"Nastąpiła próba zatrzymania tunelu, który nie został jeszcze utworzony. `{ex.GetType()}`");
                        }
                        else if (ex is BashProcessNotRunningException)
                        {
                            await message.Channel.SendMessageAsync($"Nastąpiła próba zatrzymania tunelu, który nie został jeszcze uruchomiony lub został wcześniej zatrzymany. `{ex.GetType()}`");
                        }
                        else
                        {
                            await message.Channel.SendMessageAsync($"Wystąpił nieznany błąd podczas zatrzymywania. \nTreść błędu `{ex.GetType()}`: ```{ex.Message}```");
                        }
                    }

                }
                else
                {
                    await message.Channel.SendMessageAsync($"Wstępna analiza, wykazala że tunel nie został utworzony *(stan połączenia to: `{tunnelConnectionState}`)*. \nJeśli chcesz się połączyć, wpisz `!p`.");
                }
            }
            else if (content.Contains('s')) // Connection status - "Status polaczenia"
            {
                await message.Channel.SendMessageAsync("Sprawdzanie statusu połączenia...");

                TunnelConnectionState? tunnelConnectionState = await CheckConnection();
                await message.Channel.SendMessageAsync($"Aktualny status połączenia, to: *{tunnelConnectionState}*");
            }

            await SetCurrentActivity(await CheckConnection());
        }

        /// <summary>
        /// Checks current connection with TunnelUnknownConnectionStateException catching 
        /// </summary>
        /// <returns></returns>
        private static async Task<TunnelConnectionState?> CheckConnection()
        {
            try
            {
                return await _tunnel.CheckConnectionType();
            }
            catch (TunnelUnknownConnectionStateException ex)
            {
                Console.WriteLine($"Problem while checking connection type: {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item1"></param>
        /// <returns></returns>
        private static async Task SetCurrentActivity(TunnelConnectionState? tunnelConnectionState)
        {
            Game game;
            switch (tunnelConnectionState)
            {
                case TunnelConnectionState.RemoteConnection:
                case TunnelConnectionState.LocalConnection:
                    game = new Game($"{tunnelConnectionState} in {SettingsManager.AppConfig.RemoteHost.GetPortAndIP()}", ActivityType.Playing);
                    break;
                case TunnelConnectionState.NoConnection:
                case TunnelConnectionState.StoppedWithoutChecking:
                    game = new Game(tunnelConnectionState.ToString(), ActivityType.Listening);
                    break;
                default:
                    game = null;
                    break;
            }

            await _client.SetActivityAsync(game);
        }

        /// <summary>
        /// Catching discord library logs
        /// </summary>
        /// <param name="logMessage">Log message</param>
        /// <returns></returns>
        private static Task Log(LogMessage logMessage)
        {
            Console.WriteLine($"{DateTime.Now.Date} - {logMessage}");
            

            if (_client.ConnectionState == ConnectionState.Disconnected)
            {
                Console.WriteLine($"{DateTime.Now} -> Disconnected status detected - running new Task, to check is bot still working properly!");

                Task.Run(async () =>
                {
                    await Task.Delay(10 * 1000);

                    if (_client.ConnectionState == ConnectionState.Disconnected)
                    {
                        Console.WriteLine($"{DateTime.Now} -> Process is killing itself, due to Disconnected status has not changed, after 10 seconds delay.");
                        Process.GetCurrentProcess().Kill();
                    }
                    else
                    {
                        Console.WriteLine($"{DateTime.Now} -> OK! Bot is still working.");
                    }
                });
            }

            return Task.CompletedTask;
        }
    }
}
