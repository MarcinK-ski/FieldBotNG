﻿using System;
using System.Threading.Tasks;
using TunnelingTools;

namespace ReverseSSHTunnelExample
{
    class Program
    {
        static async Task Main()
        {
            BashTools.BashProcess.IsWSL = true;

            ReverseSSHTunnel reverseSSHTunnel = new ReverseSSHTunnel(SettingsManager.AppConfig.RemoteHost, SettingsManager.AppConfig.LocalHost);
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
