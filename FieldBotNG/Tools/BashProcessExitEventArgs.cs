using System;

namespace FieldBotNG.Tools
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