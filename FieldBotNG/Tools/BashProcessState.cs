﻿namespace FieldBotNG
{
    public enum BashProcessState
    {
        Prepared,
        OK,
        NotStarted,
        Exited,
        ExitedWithError,
        EmptyBashCommand,
        OldProcessIsRunning,
        UndefinedError
    }
}