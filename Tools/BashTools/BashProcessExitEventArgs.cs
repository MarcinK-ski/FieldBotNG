using System;

namespace BashTools
{
    public class BashProcessExitEventArgs : EventArgs
    {
        public BashProcessState ProcessState { get; }
        public int? CodeError { get; }

        public BashProcessExitEventArgs(BashProcessState processState, int? exitProcessCode)
        {
            ProcessState = processState;
            CodeError = exitProcessCode;
        }
    }
}