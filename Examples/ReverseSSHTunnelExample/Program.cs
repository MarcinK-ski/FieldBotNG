using FieldBotNG;
using FieldBotNG.Tools;
using System;
using System.Threading.Tasks;

namespace ReverseSSHTunnelExample
{
    class Program
    {
        static async Task Main()
        {
            ReverseSSHTunnel reverseSSHTunnel = new ReverseSSHTunnel(Helper.AppConfig.RemoteHost, Helper.AppConfig.LocalHost);
            reverseSSHTunnel.Start();

            TunnelConnectionState currentConnectionState = await reverseSSHTunnel.CheckConnectionType();
            Console.WriteLine($"\nTunnel has been started. Current connection state => {currentConnectionState}");

            Console.WriteLine("\nClick any key, to stop tunnel...");
            Console.ReadKey();

            currentConnectionState = await reverseSSHTunnel.Stop();
            Console.WriteLine($"\nTunnel has been stopped. Current connection state => {currentConnectionState}");

            Console.WriteLine("\nClick any key, to exit demo app...");
            Console.ReadKey();
        }
    }
}
