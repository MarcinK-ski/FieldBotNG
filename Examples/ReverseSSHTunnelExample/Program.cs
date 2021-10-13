using System;
using System.Threading.Tasks;
using TunnelingTools;

namespace ReverseSSHTunnelExample
{
    class Program
    {
        static async Task Main()
        {
            BashTools.BashProcess.IsWSL = SettingsManager.AppConfig.WSL;

            int hostsLength = SettingsManager.AppConfig.Hosts.Length;
            if (hostsLength == 0)
            {
                Console.WriteLine("ERROR: There's no hosts in config file!!!");
            }
            else if (hostsLength == 1)
            {
                await CreateConnectionAsync();
            }
            else
            {
                Console.WriteLine($"There are {hostsLength} hosts - select correct index and press ENTER:");
                for (int i = 0; i < hostsLength; i++)
                {
                    var hostsInfo = SettingsManager.AppConfig.Hosts[i];

                    Console.WriteLine($"[{i}]: {hostsInfo.LocalHost} -> {hostsInfo.RemoteHost}");
                }

                string selectedIndex = Console.ReadLine();
                bool isIntegerOK = int.TryParse(selectedIndex, out int index);
                index = isIntegerOK ? index : hostsLength - 1;

                Console.WriteLine($"Creating connection for [{index}]");

                await CreateConnectionAsync(index);
            }

            Console.WriteLine("\nClick any key, to exit demo app...");
            Console.ReadKey();
        }

        static async Task CreateConnectionAsync(int hostIndex = 0)
        {
            ReverseSSHTunnel reverseSSHTunnel = 
                new ReverseSSHTunnel(SettingsManager.AppConfig.Hosts[hostIndex].RemoteHost, 
                                     SettingsManager.AppConfig.Hosts[hostIndex].LocalHost);

            await reverseSSHTunnel.Start();

            TunnelConnectionState currentConnectionState = await reverseSSHTunnel.CheckAndUpdateConnectionType();
            Console.WriteLine($"\nTunnel has been started. Current connection state => {currentConnectionState}");

            Console.WriteLine("\nClick any key, to stop tunnel...");
            Console.ReadKey();

            currentConnectionState = (await reverseSSHTunnel.Stop()).TunnelConnectionState;
            Console.WriteLine($"\nTunnel has been stopped. Current connection state => {currentConnectionState}");

        }
    }
}
