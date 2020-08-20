namespace BashTools
{
    public enum BashProcessState
    {
        Prepared,
        OK,
        NotStarted,
        Exited,
        KilledManually,
        ExitedWithError,
        EmptyBashCommand,
        OldProcessIsRunning,
        UndefinedError
    }
}