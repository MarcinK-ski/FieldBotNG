using System;

namespace BashTools
{
    /// <summary>
    /// Contains event data when process has been exited
    /// </summary>
    public class BashProcessExitEventArgs : EventArgs
    {
        /// <summary>
        /// Exit process state
        /// </summary>
        public BashProcessState ProcessState { get; }

        /// <summary>
        /// Exit code error
        /// </summary>
        public int? CodeError { get; }

        /// <summary>
        /// Initializes new instance of BashProcessExitEventArgs
        /// </summary>
        /// <param name="processState">Exit process state</param>
        /// <param name="exitProcessCode">Exit code error</param>
        public BashProcessExitEventArgs(BashProcessState processState, int? exitProcessCode)
        {
            ProcessState = processState;
            CodeError = exitProcessCode;
        }
    }
}