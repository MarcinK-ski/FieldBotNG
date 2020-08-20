namespace BashTools
{
    /// <summary>
    /// Enum with last know states of the process
    /// </summary>
    public enum BashProcessState
    {
        /// <summary>
        /// Process was prepared
        /// </summary>
        Prepared,

        /// <summary>
        /// Process was started
        /// </summary>
        OK,

        /// <summary>
        /// Process wasn't started
        /// </summary>
        NotStarted,

        /// <summary>
        /// Process has been exited
        /// </summary>
        Exited,

        /// <summary>
        /// Process has been killed manually
        /// </summary>
        KilledManually,

        /// <summary>
        /// Process has been exited with error code
        /// </summary>
        ExitedWithError,

        /// <summary>
        /// Empty string was put as command
        /// </summary>
        EmptyBashCommand,

        /// <summary>
        /// Old process is running, so new can't be prepared
        /// </summary>
        OldProcessIsRunning,

        /// <summary>
        /// Undefined error
        /// </summary>
        UndefinedError
    }
}