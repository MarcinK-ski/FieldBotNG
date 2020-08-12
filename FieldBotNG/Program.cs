namespace FieldBotNG
{
    class Program
    {
        public static void Main()
            => new DiscordConnection().MainAsync().GetAwaiter().GetResult();
    }
}
