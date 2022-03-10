public sealed class SubscribeCommand : Command<Settings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var ably = new AblyRealtime(settings.AblyConnection);
        var channel = ably.Channels.Get(
            settings.Channel,
            new ChannelOptions { 
                Params = new ChannelParams {{ "rewind", "1m" }} 
            });
        
        var welcomeRule = new Rule("[red]Welcome to Console Chat![/]").Border(BoxBorder.Double);
        welcomeRule.Alignment = Justify.Left;
        AnsiConsole.Write(welcomeRule);

        var channelRule = new Rule($"You've subscribed to the {settings.Channel} channel.");
        channelRule.Alignment = Justify.Left;
        AnsiConsole.Write(channelRule);
        
        var chatMessageQueue = new Queue<ChatMessage>();
        channel.Subscribe(message =>
        {
            var chatMessage = ((JObject)message.Data).ToObject<ChatMessage>();
            chatMessageQueue.Enqueue(chatMessage);
        });
 
        while(true)
        {
            if (chatMessageQueue.TryDequeue(out ChatMessage chatMessage))
            {
                var panel = new Panel(chatMessage.Message)
                    .Header(new PanelHeader(chatMessage.Name, Justify.Left))
                    .BorderColor(ConvertStringToColor(chatMessage.Color));
                AnsiConsole.Write(panel);
            }
        }

        return 0;
    }

    private Color ConvertStringToColor(string color)
    {
        if (Enum.TryParse<ConsoleColor>(color, true, out ConsoleColor consoleColor))
        {
            return Color.FromConsoleColor(consoleColor);
        }

        return Color.White;
    }
}