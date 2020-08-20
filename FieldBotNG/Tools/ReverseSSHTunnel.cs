using FieldBotNG.Settings;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FieldBotNG.Tools
{
    /// <summary>
    /// Class to create Reverse SSH Tunneling
    /// </summary>
    public class ReverseSSHTunnel
    {
        /// <summary>
        /// Last BashProcess command, used to create Reverse SSH Tunnel
        /// </summary>
        public string LastStartedCommandSSH { get; private set; }

        /// <summary>
        /// Reverse SSH tunnel process
        /// </summary>
        private BashProcess _SSHProcess;

        private TunnelSettings _remoteHost;
        /// <summary>
        /// Gets or sets Remote Host.
        /// Pay atention! It can't be changed, while tunnel is established!
        /// </summary>
        public TunnelSettings RemoteHost 
        {
            get
            {
                return _remoteHost;
            }
            set
            {
                if (!IsTunnelEstablished)
                {
                    _remoteHost = value;
                }
                else
                {
                    throw new TunnelEstablishedException("You cannot change remote host, while connection is established.");
                }
            }
        }

        private TunnelSettings _localSideHost;
        /// <summary>
        /// Gets or sets Local Side Host.
        /// Pay atention! It can't be changed, while tunnel is established!
        /// </summary>
        public TunnelSettings LocalSideHost
        {
            get
            {
                return _localSideHost;
            }
            set
            {
                if (!IsTunnelEstablished)
                {
                    _localSideHost = value;
                }
                else
                {
                    throw new TunnelEstablishedException("You cannot change local side host, while connection is established.");
                }
            }
        }

        /// <summary>
        /// Is tunnel connection established
        /// </summary>
        public bool IsTunnelEstablished { get; private set; }

        /// <summary>
        /// Creates new ReverseSSHTunnel object
        /// </summary>
        /// <param name="remoteHost">Remote host - visible in internet by IP</param>
        /// <param name="localSideHost">Local side host - visible or invisible in internet by IP</param>
        public ReverseSSHTunnel(TunnelSettings remoteHost, TunnelSettings localSideHost)
        {
            RemoteHost = remoteHost;
            LocalSideHost = localSideHost;
        }

        /// <summary>
        /// Runs Reverse SSH Tunnel, using command: "ssh -NvR bindAddress:remoteAccessPort:localSideHost:localSidePort user@remoteHost
        /// </summary>
        /// <param name="bindAddress">
        /// By default, the listening socket on the server will be bound to the loopback interface only. 
        /// This may be overridden by specifying a bind_address. 
        /// An empty bind_address, or the address '*', indicates that the remote socket should listen on all interfaces. 
        /// Specifying a remote bind_address will only succeed if the server's GatewayPorts option is enabled.
        /// </param>
        /// <returns></returns>
        public bool Start(string bindAddress = "0.0.0.0")
        {
            LastStartedCommandSSH = $"ssh -NvR {bindAddress}:{RemoteHost.Port}:{LocalSideHost.IP}:{LocalSideHost.Port} {RemoteHost.User}@{RemoteHost.IP}";
            _SSHProcess = new BashProcess(LastStartedCommandSSH);

            _SSHProcess.StandardOutputStringReceived += _SSHProcess_StandardOutputStringReceived;
            _SSHProcess.StandardErrorStringReceived += _SSHProcess_StandardOutputStringReceived;

            IsTunnelEstablished = _SSHProcess.RunNewProcess(true, true, true);
            return IsTunnelEstablished;
        }

        /// <summary>
        /// Handle StdOutput/StdError
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="stdEventArgs"></param>
        private void _SSHProcess_StandardOutputStringReceived(object sender, BashProcessStdEventArgs stdEventArgs)
        {
            string output = stdEventArgs.Output;
            Console.WriteLine($">{output}");

            /* TODO:
             * Enter password when:
             *  - StdInput is true
             *  - Output is not null/empty
             *  - Password prompt
             */

            /*            
            if (_SSHProcess.CurrentBashProcess.StartInfo.RedirectStandardInput && !string.IsNullOrWhiteSpace(output))
            {
                string user = Regex.Escape(RemoteHost.User);
                string ip = Regex.Escape(RemoteHost.IP);

                CheckIsPasswordPromptAndInputIt(output, $@"{user}\@{ip}\'s password:");
            }
            */
        }

        /// <summary>
        /// Check is StdOutput or StdError contains password prompt, if yes, put it to StdInput
        /// </summary>
        protected void CheckIsPasswordPromptAndInputIt(string outputOrErrorData, string passwordRegex)
        {
            Regex regex = new Regex(passwordRegex);
            Match match = regex.Match(outputOrErrorData);
            if (match.Success)
            {
                _SSHProcess.WriteToStandardInput(RemoteHost.Password);
            }
        }

        /// <summary>
        /// Stop tunnel
        /// </summary>
        public async Task<TunnelConnectionState> Stop()
        {
            _SSHProcess.KillProcess();
            IsTunnelEstablished = !_SSHProcess.IsProcessRunning;
            if (await CheckConnectionType() != TunnelConnectionState.NoConnection)
            {
                List<int> PIDs = BashProcess.FindPIDs(LastStartedCommandSSH);
                foreach (int pid in PIDs)
                {
                    BashProcess.KillProcess(pid);
                }
            }

            return await CheckConnectionType();
        }

        /// <summary>
        /// Checking connection type (local/remote) / state (connected/disconnected)
        /// (To execute this method, device must be ssh authorised_host (ssh can't ask for password))
        /// </summary>
        /// <returns>Tunnel connection state or throws TunnelUnknownConnectionStateException when netstat has no result.</returns>
        public async Task<TunnelConnectionState> CheckConnectionType()
        {
            string netstatLocalAddress = null;

            BashProcess netstatProcess = new BashProcess($"ssh {RemoteHost.User}@{RemoteHost.IP} netstat -tlnp")
            {
                SubscribeStandardOutput = false
            };

            string netstatResult = await netstatProcess.RunNewProcessAndReadStdOutputAsync();

            if (!string.IsNullOrWhiteSpace(netstatResult))
            {
                Regex regex = new Regex($@"[0-255].[0-255].[0-255].[0-255](?=:{RemoteHost.Port})");
                Match match = regex.Match(netstatResult);
                if (match.Success)
                {
                    netstatLocalAddress = match.Groups[0].Value;
                }

                switch (netstatLocalAddress)
                {
                    case "0.0.0.0":
                        IsTunnelEstablished = true;
                        return TunnelConnectionState.RemoteConnection;
                    case "127.0.0.1":
                        IsTunnelEstablished = true;
                        return TunnelConnectionState.LocalConnection;
                    case null:
                        IsTunnelEstablished = false;
                        return TunnelConnectionState.NoConnection;
                    default:
                        throw new TunnelUnknownConnectionStateException($"Unknown Tunnel Connection State: {netstatLocalAddress}");
                }
            }

            throw new TunnelUnknownConnectionStateException("Unknown Tunnel Connection State: no netstatResult");
        }
    }
}
