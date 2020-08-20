namespace FieldBotNG
{
    class Program
    {
        public static void Main()
            => new DiscordBotCore().MainAsync().GetAwaiter().GetResult();
    }
}
