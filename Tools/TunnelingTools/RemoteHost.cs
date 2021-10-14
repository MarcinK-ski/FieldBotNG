namespace TunnelingTools
{
    /// <summary>
    /// Class with fields for remote host (where you connecting to)
    /// </summary>
    public class RemoteHost : Host
    {
        /// <summary>
        /// Host user
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// Host password
        /// </summary>
        public string Password { get; set; }

        public override string ToString()
        {
            return $"RHost: {User}@{IP}:{Port}";
        }
    }
}
