using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FieldBotNG.Tools
{
    /// <summary>
    /// Class to create and manage Bash process
    /// </summary>
    public partial class BashProcess
    {
        /// <summary>
        /// Represents method that will handle StandardOutputStringReceived and StandardErrorStringReceived methods.
        /// </summary>
        /// <param name="sender">Source of stream event</param>
        /// <param name="stdEventArgs">Data from StdOutput/StdError stream</param>
        public delegate void StdEventHandler(object sender, BashProcessStdEventArgs stdEventArgs);

        public delegate void ExitProcessEventHandler(object sender, BashProcessExitEventArgs exitProcessEventArgs);

        /// <summary>
        /// Occurs when process redirect line to Process's StdOutput stream
        /// </summary>
        public event StdEventHandler StandardOutputStringReceived;

        /// <summary>
        /// Occurs when process redirect line to Process's StdError stream
        /// </summary>
        public event StdEventHandler StandardErrorStringReceived;

        public event ExitProcessEventHandler ExitedProcess;

        private string _bashCommand;
        /// <summary>
        /// Gets or sets bash command to execute in process.
        /// Set is automatically using Replace("\"", "\\\"") on it.
        /// </summary>
        public string BashCommand {
            get
            {
                return _bashCommand;
            }
            set
            {
                _bashCommand = value.Replace("\"", "\\\"");
            }
        }

        /// <summary>
        /// Last prepared/started process
        /// </summary>
        public Process CurrentBashProcess { get; protected set; }

        /// <summary>
        /// True if process is running via WSL, false if on Linux machine
        /// </summary>
        public bool IsWSL { get; set; }

        /// <summary>
        /// Has process been started or is still running.
        /// </summary>
        public bool IsProcessRunning { get; protected set; }

        /// <summary>
        /// Detailed information about process current state
        /// </summary>
        public BashProcessState ProcessState { get; protected set; }

        /// <summary>
        /// Last exit process code
        /// </summary>
        public int? ExitProcessCode { get; protected set; }


        /// <summary>
        /// Creates new BashProcess object
        /// </summary>
        /// <param name="bashCommand">Bash command to execute (Automatically using Replace("\"", "\\\"") on it).</param>
        /// <param name="isWSLMode">True if process is running via WSL (require installed WSL on machine!), false if on Linux machine</param>
        public BashProcess(string bashCommand, bool isWSLMode = false)
        {
            BashCommand = bashCommand;
            IsWSL = isWSLMode;
        }

        /// <summary>
        /// Runs process and reads standard output and wait for end of process executing.
        /// </summary>
        /// <param name="processTimeout">Time (in ms) to wait before SIGHTERM proces. If 0, process won't be killed - method will wait for exit</param>
        /// <returns>Process's standard output result. Null if process doesn't even start</returns>
        public async Task<string> RunNewProcesAndReadStdOutput(int processTimeout = 0)
        {
            string stdOutputResult = null;

            if (RunNewProcess(true))
            {
                stdOutputResult = await CurrentBashProcess.StandardOutput.ReadToEndAsync();

                WaitForFinish(processTimeout);
            }

            return stdOutputResult;
        }

        /// <summary>
        /// Runs new process and just wait for finish.
        /// </summary>
        /// <param name="processTimeout">Time (in ms) to wait before SIGHTERM proces. If 0, process won't be killed - method will wait for exit</param>
        /// <returns>Has process been started succesfully</returns>
        public bool RunNewProcessAndWaitForFinish(int processTimeout = 0)
        {
            bool isProcessStarted = RunNewProcess();
            WaitForFinish(processTimeout);

            return isProcessStarted;
        }

        /// <summary>
        /// Waits for process finish or terminate after processTimeout
        /// </summary>
        /// <param name="processTimeout">Time (in ms) to wait before SIGHTERM proces. If 0, process won't be killed - method will wait for exit</param>
        private void WaitForFinish(int processTimeout)
        {
            if (processTimeout > 0)
            {
                bool isExited = CurrentBashProcess.WaitForExit(processTimeout);

                if (!isExited)
                {
                    CurrentBashProcess.Kill();
                }
            }
            else
            {
                CurrentBashProcess.WaitForExit();
            }
        }

        /// <summary>
        /// Runs new process
        /// </summary>
        /// <param name="stdOutput">If true, StandardOutput and StandardError is accessable from </param>
        /// <returns>True if process was prepared and started, false if not.</returns>
        public bool RunNewProcess(bool stdOutput = false, bool stdError = false, bool stdInput = false)
        {
            bool isProcessPrepared = PrepareBashProcess(stdOutput, stdError, stdInput);

            if (isProcessPrepared)
            {
                CurrentBashProcess.Exited += CurrentBashProcess_Exited;

                SetStateAfterProcessStarted(CurrentBashProcess.Start());

                if (IsProcessRunning)
                {
                    if (stdOutput)
                    {
                        CurrentBashProcess.BeginOutputReadLine();
                    }

                    if (stdError)
                    {
                        CurrentBashProcess.BeginErrorReadLine();
                    }

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Escapes quotation marks and sets new process as current (/bin/bash)
        /// </summary>
        /// <param name="stdOutput"></param>
        /// <param name="stdError"></param>
        /// <param name="stdInput"></param>
        /// <returns>Is process correctly prepared</returns>
        protected bool PrepareBashProcess(bool stdOutput, bool stdError, bool stdInput)
        {
            if (string.IsNullOrWhiteSpace(BashCommand))
            {
                Console.WriteLine("Bash command could not be null or empty or whitespace");
                ProcessState = BashProcessState.EmptyBashCommand;
            }
            else if (IsProcessRunning)
            {
                Console.WriteLine("Stop process first");
                ProcessState = BashProcessState.OldProcessIsRunning;
            }
            else
            {
                try
                {
                    string fileName = IsWSL 
                                        ? "wsl" 
                                        : "/bin/bash";
                    string args = IsWSL 
                                    ? $"-e {BashCommand}" 
                                    : $"-c \"{BashCommand}\"";

                    CurrentBashProcess = new Process()
                    {
                        StartInfo = new ProcessStartInfo()
                        {
                            FileName = fileName,
                            Arguments = args,
                            RedirectStandardOutput = stdOutput,
                            RedirectStandardError = stdError,
                            RedirectStandardInput = stdInput,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        }
                    };

                    if (stdOutput)
                    {
                        CurrentBashProcess.OutputDataReceived += CurrentBashProcess_OutputDataReceived;
                    }

                    if (stdError)
                    {
                        CurrentBashProcess.ErrorDataReceived += CurrentBashProcess_ErrorDataReceived;
                    }

                    ExitProcessCode = null;
                    IsProcessRunning = false;
                    ProcessState = BashProcessState.Prepared;
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    ProcessState = BashProcessState.UndefinedError;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets IsProcessRunning (true/false) and ProcessState (OK/NotStarted) depends on isStarted param
        /// </summary>
        /// <param name="isStarted">Inform, is process started successfully or not.</param>
        private void SetStateAfterProcessStarted(bool isStarted)
        {
            if (isStarted)
            {
                IsProcessRunning = true;
                ProcessState = BashProcessState.OK;
            }
            else
            {
                IsProcessRunning = false;
                ProcessState = BashProcessState.NotStarted;
            }
        }

        /// <summary>
        /// Kill current process and unsubscribe events
        /// </summary>
        public void KillProcess()
        {
            CurrentBashProcess.Exited -= CurrentBashProcess_Exited;

            CurrentBashProcess.Kill();
            CurrentBashProcess.WaitForExit();
            CurrentBashProcess.Dispose();

            ExitedOrKilledProcessAction(true);
        }

        /// <summary>
        /// Kill process by PID
        /// </summary>
        /// <param name="pid">Process ID</param>
        /// <returns>True if process has been started succesfully, false if not.</returns>
        public static bool KillProcess(int pid)
        {
            BashProcess killer = new BashProcess($"kill -9 {pid}", Helper.AppConfig.WSL);
            return killer.RunNewProcessAndWaitForFinish();
        }

        /// <summary>
        /// Action called on process Exit or Kill 
        /// </summary>
        /// <param name="isKilled">Has process been killed</param>
        protected void ExitedOrKilledProcessAction(bool isKilled)
        {
            IsProcessRunning = false;
            ExitProcessCode = CurrentBashProcess.ExitCode;

            if (isKilled)
            {
                ProcessState = BashProcessState.KilledManually;
                ExitedProcess?.Invoke(this, new BashProcessExitEventArgs(ProcessState, null));
            }
            else
            {
                if (ExitProcessCode == 0)
                {
                    ProcessState = BashProcessState.Exited;
                }
                else
                {
                    ProcessState = BashProcessState.ExitedWithError;
                }

                ExitedProcess?.Invoke(this, new BashProcessExitEventArgs(ProcessState, ExitProcessCode));
            }
        }

        /// <summary>
        /// Changes object properties, to inform that current process has been exited
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CurrentBashProcess_Exited(object sender, EventArgs e)
        {
            ExitedOrKilledProcessAction(false);
        }

        /// <summary>
        /// Invokes StandardOutputStringReceived event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CurrentBashProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            StandardOutputStringReceived?.Invoke(this, new BashProcessStdEventArgs(e.Data));
        }

        /// <summary>
        /// Invokes StandardErrorStringReceived event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentBashProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            StandardErrorStringReceived?.Invoke(this, new BashProcessStdEventArgs(e.Data));
        }
    }
}
