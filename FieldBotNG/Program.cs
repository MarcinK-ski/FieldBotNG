using BashTools;
using Discord;
using Discord.WebSocket;
using FieldBotNG.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
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
        private static List<ReverseSSHTunnel> _tunnels;

        private static Dictionary<TunnelConnectionState, int> _connectionsStateCounters;

        private static int _defeultDeviceIndex = -1;

        static async Task Main()
        {
            DisplayHelloInfo();

            BashProcess.IsWSL = SettingsManager.AppConfig.WSL;

            int hostsLength = SettingsManager.AppConfig.Hosts.Length;
            if (hostsLength == 0)
            {
                Console.WriteLine("ERROR: There's no hosts in config file!!!");
            }
            else
            {
                Console.WriteLine($"There are {hostsLength} hosts.");

                _tunnels = new List<ReverseSSHTunnel>();
                _connectionsStateCounters = new Dictionary<TunnelConnectionState, int>();

                foreach (var item in Enum.GetValues(typeof(TunnelConnectionState)).Cast<TunnelConnectionState>())
                {
                    _connectionsStateCounters.TryAdd(item, 0);
                }

                for (int i = 0; i < hostsLength; i++)
                {
                    EndToEndHosts hostsInfo = SettingsManager.AppConfig.Hosts[i];

                    Console.WriteLine($"Adding E2E info to list with index [{i}]: {hostsInfo.LocalHost} -> {hostsInfo.RemoteHost}");

                    if (_defeultDeviceIndex == -1 && hostsInfo.IsDefault)
                    {
                        _defeultDeviceIndex = i;
                    }

                    ReverseSSHTunnel tempTunnel = new ReverseSSHTunnel(hostsInfo.RemoteHost, hostsInfo.LocalHost);
                    TunnelDestroyResponse tunnelConnectionState = await tempTunnel.CheckAndKillOldProcesses();
                    Console.WriteLine($"{DateTime.Now} -> Current tunnel state is: {tunnelConnectionState.TunnelConnectionState}");

                    if (!string.IsNullOrWhiteSpace(tunnelConnectionState.ExceptionMessage))
                    {
                        Console.WriteLine($"{DateTime.Now} -> Error while killing processes: {tunnelConnectionState.ExceptionMessage}");
                    }

                    _tunnels.Add(tempTunnel);
                    _connectionsStateCounters[tunnelConnectionState.TunnelConnectionState]++;
                }

                if (_defeultDeviceIndex == -1)
                {
                    _defeultDeviceIndex = 0;
                }
            }


            _client = new DiscordSocketClient();

            _client.Log += Log;
            _client.MessageReceived += MessageReceived;

            await _client.LoginAsync(TokenType.Bot, SettingsManager.AppConfig.DiscordBot.Token);
            await _client.StartAsync();
            await UpdateCurrentActivity();

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
                if (_defeultDeviceIndex == -1)
                {
                    await message.Channel.SendMessageAsync("W konfiguracji aplikacji nie ma podanych hostów typu E2E!");
                }
                else if (SettingsManager.AppConfig.DiscordBot.AllowedChannels.Contains(message.Channel.Id))
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
                                                                    // TODO: pamiętać o: allowedUsers oraz isAnonymous!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            string content = message.Content.Trim().ToLower();

            // TODO: Przerobić na regex, bo jakoś trzeba rozróżniać, czy podany został!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (content.Contains("pomoc")) // Help
            {
                StringBuilder helpContent = new StringBuilder();

                helpContent.Append("Każda komenda musi zaczynać się znakiem: `!` *(wykrzyknik)*");
                helpContent.Append($"\nLista komend: ");
                helpContent.Append($"\n    - `p` - otwiera połączenie z domyślnym urządzeniem (tu: rejestratorem), ");
                helpContent.Append($"\n    - `p XYZ` - otwiera połączenie z urządzeniem o indeksie XYZ, ");
                helpContent.Append($"\n    - `r` - zamyka połączenie z domyślnym urządzeniem (tu: rejestratorem), ");
                helpContent.Append($"\n    - `r XYZ` - zamyka połączenie z urządzeniem o indeksie XYZ, ");
                helpContent.Append($"\n    - `s` - sprawdza, czy połączenie z domyślnym urządzeniem (tu: rejestratorem) jest aktywne, ");
                helpContent.Append($"\n    - `s XYZ` - sprawdza, czy połączenie z urządzeniem o indeksie XYZ jest aktywne, ");
                helpContent.Append($"\n    - `w` - wyświetla wszystkie aktywne połączenia i indeksy urządzeń (te do których nie masz dostępu, zostaną \"zanonimizowane\"), ");
                helpContent.Append($"\n    - `u` - wyświetla wszystkie dostępne urządzenia i ich indeks (te do których nie masz dostępu, zostaną \"zanonimizowane\"), ");
                helpContent.Append($"\n    - `pomoc` - wyświetla pomoc, czyli tę listę komend.");

                await message.Channel.SendMessageAsync(helpContent.ToString());
            }
            else if (content.Contains('p'))  // Connect - "Polacz"
                                             // TODO: Uwzględnić AllowedUsers!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            {
                if (_tunnels[_defeultDeviceIndex].IsTunnelEstablished)
                {
                    await message.Channel.SendMessageAsync("Tunel już istnieje. Stan możesz sprawdzić wpisując `!s`. \nJeśli po wpisaniu `!s`, pojawi się informacja o braku połączenia, spróbuj jeszcze raz.");
                }
                else
                {
                    await message.Channel.SendMessageAsync("Próba utworzenia tunelu...");

                    bool isTunnelStarted = await _tunnels[_defeultDeviceIndex].Start();

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
                                            // TODO: Uwzględnić AllowedUsers!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            {
                await message.Channel.SendMessageAsync("Czekaj...");

                TunnelConnectionState? tunnelConnectionState = await CheckConnection(_defeultDeviceIndex);

                if (_tunnels[_defeultDeviceIndex].IsTunnelEstablished)
                {
                    try
                    {
                        TunnelDestroyResponse stoppingResult = await _tunnels[_defeultDeviceIndex].Stop();
                        tunnelConnectionState = stoppingResult.TunnelConnectionState;

                        if (tunnelConnectionState == TunnelConnectionState.NoConnection)
                        {
                            await message.Channel.SendMessageAsync("Pomyślnie rozłączono!");
                        }
                        else if (tunnelConnectionState == TunnelConnectionState.StoppedWithoutChecking)
                        {
                            await message.Channel.SendMessageAsync($"Rozłączono, ale nie wystąpiło sprawdzenie istniejących połączeń. Treść błędu: {stoppingResult.ExceptionMessage}");
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

                TunnelConnectionState? tunnelConnectionState = await CheckConnection(_defeultDeviceIndex);
                await message.Channel.SendMessageAsync($"Aktualny status połączenia, to: *{tunnelConnectionState}*");
            }

            await UpdateCurrentActivity();
        }

        private static async Task CheckAndUpdateAllConnectionsState()
        {
            for (int i = 0; i < _tunnels.Count; i++)
            {
                await CheckConnection(i);
            }
        }

        /// <summary>
        /// Checks current connection with TunnelUnknownConnectionStateException catching 
        /// </summary>
        /// <returns></returns>
        private static async Task<TunnelConnectionState?> CheckConnection(int index)
        {
            if (index < 0)
            {
                index = _defeultDeviceIndex;
            }

            try
            {
                ReverseSSHTunnel tunnel = _tunnels[index];

                TunnelConnectionState lastConnetcionState = tunnel.LastTunnelConnectionState;
                TunnelConnectionState currentConnectionState = await tunnel.CheckAndUpdateConnectionType();

                if (currentConnectionState != lastConnetcionState)
                {
                    if (_connectionsStateCounters[lastConnetcionState] > 0)
                    {
                        _connectionsStateCounters[lastConnetcionState]--;
                    }

                    _connectionsStateCounters[currentConnectionState]++;
                }

                return currentConnectionState;
            }
            catch (TunnelUnknownConnectionStateException ex)
            {
                Console.WriteLine($"Problem while checking connection type: {ex.Message}");
                return TunnelConnectionState.Unknown;
            }
        }


        /// <summary>
        /// Sets text for discord's "Activity"
        /// </summary>
        /// <remarks>
        /// Activity will contain "NoConnection" when any tunnel is opened. Other states will be listed with counter of them.
        /// </remarks>
        private static async Task UpdateCurrentActivity()
        {
            Game game;

            if (_connectionsStateCounters?.Count == 0)
            {
                game = new Game("Błąd konfiguracji! Brak hostów E2E!", ActivityType.CustomStatus);
            }
            else
            {
                TunnelConnectionState tunnelConnectionState = _tunnels[_defeultDeviceIndex].LastTunnelConnectionState;

                switch (tunnelConnectionState)
                {
                    case TunnelConnectionState.RemoteConnection:
                    case TunnelConnectionState.LocalConnection:
                        game = new Game($"{tunnelConnectionState} in {_tunnels[_defeultDeviceIndex].RemoteHost.GetPortAndIP()}", ActivityType.Playing);
                        break;
                    case TunnelConnectionState.NoConnection:
                    case TunnelConnectionState.StoppedWithoutChecking:
                        game = new Game(tunnelConnectionState.ToString(), ActivityType.Listening);
                        break;
                    default:
                        game = null;
                        break;
                }

                // todo: use field: _connectionsStateCounters -> stringi mają być typu "X połączeń typu Remote, X połączeń typu local", a jeśli nie ma żadnego, to po prostu jedno NoConnection
                /*foreach (var tunnel in _tunnels)
                {
                    TunnelConnectionState tunnelConnectionState = tunnel.LastTunnelConnectionState;

                    switch (tunnelConnectionState)
                    {
                        case TunnelConnectionState.RemoteConnection:
                        case TunnelConnectionState.LocalConnection:
                            game = new Game($"{tunnelConnectionState} in {tunnel.RemoteHost.GetPortAndIP()}", ActivityType.Playing);
                            break;
                        case TunnelConnectionState.NoConnection:
                        case TunnelConnectionState.StoppedWithoutChecking:
                            game = new Game(tunnelConnectionState.ToString(), ActivityType.Listening);
                            break;
                        default:
                            game = null;
                            break;
                    } 
                }*/
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
