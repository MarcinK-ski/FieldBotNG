namespace TunnelingTools
{
    /// <summary>
    /// Class with properties for visible-for-you-host
    /// </summary>
    public class LocalHost : Host
    {
        public override string ToString()
        {
            return $"LHost: {IP}:{Port}";
        }
    }
}
