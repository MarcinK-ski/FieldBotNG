namespace BashTools
{
    /// <summary>
    /// Linux signals from `kill` command
    /// </summary>
    public enum BashProcessKillSignals
    {
        SIGHUP = 1,
        SIGINT,
        SIGQUIT,
        SIGILL,
        SIGTRAP,
        SIGABRT,
        SIGBUS,
        SIGFPE,
        /// <summary>
        /// Brutal signal to kill
        /// </summary>
        SIGKILL,
        SIGUSR1,
        SIGSEGV,
        SIGUSR2,
        SIGPIPE,
        SIGALRM,
        /// <summary>
        /// Default signal
        /// </summary>
        SIGTERM,
        SIGSTKFLT,
        SIGCHLD,
        SIGCONT,
        SIGSTOP,
        SIGTSTP,
        SIGTTIN,
        SIGTTOU,
        SIGURG,
        SIGXCPU,
        SIGXFSZ,
        SIGVTALRM,
        SIGPROF,
        SIGWINCH,
        SIGIO,
        SIGPWR,
        SIGSYS,
        SIGRTMIN = 34,
        SIGRTMIN1,
        SIGRTMIN2,
        SIGRTMIN3,
        SIGRTMIN4,
        SIGRTMIN5,
        SIGRTMIN6,
        SIGRTMIN7,
        SIGRTMIN8,
        SIGRTMIN9,
        SIGRTMIN10,
        SIGRTMIN11,
        SIGRTMIN12,
        SIGRTMIN13,
        SIGRTMIN14,
        SIGRTMIN15,
        SIGRTMAX14,
        SIGRTMAX13,
        SIGRTMAX12,
        SIGRTMAX11,
        SIGRTMAX10,
        SIGRTMAX9,
        SIGRTMAX8,
        SIGRTMAX7,
        SIGRTMAX6,
        SIGRTMAX5,
        SIGRTMAX4,
        SIGRTMAX3,
        SIGRTMAX2,
        SIGRTMAX1,
        SIGRTMAX
    }
}