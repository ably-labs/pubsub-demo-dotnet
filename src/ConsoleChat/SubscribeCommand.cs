public sealed class SubscribeCommand : Command<Settings>
{
    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var name = DrawConsoleAndGetName(settings);

        var clientOptions = new ClientOptions(settings.AblyApiKey) { ClientId = name };
        var ably = new AblyRealtime(clientOptions);
        var channel = ably.Channels.Get(
            settings.Channel,
            new ChannelOptions
            {
                Params = new ChannelParams { { "rewind", "2m" } }
            });

        var consoleMessageQueue = new Queue<ConsoleMessage>();

        channel.Presence.Subscribe(member => {
            ConsoleMessage? presenceMessage = null;
            switch (member.Action)
            {
                case PresenceAction.Enter:
                    presenceMessage = new ConsoleMessage("Presence", $"{member.ClientId} has joined as {member.Data}.", "White");
                    break;
                case PresenceAction.Leave:
                     presenceMessage = new ConsoleMessage("Presence", $"{member.ClientId} has left.", "White");
                    break;
                default:
                    break;
            }

            if (presenceMessage != null)
            {
                consoleMessageQueue.Enqueue(presenceMessage);
            }
        });
        channel.Presence.Enter("subscriber");

        channel.Subscribe(message =>
        {
            var consoleMessage = message.Data.ToObject<ConsoleMessage>();
            consoleMessageQueue.Enqueue(consoleMessage);
        });

        while (true)
        {
            if (consoleMessageQueue.TryDequeue(out ConsoleMessage? consoleMessage))
            {
                var panel = new Panel(consoleMessage.Message)
                    .Header(new PanelHeader(consoleMessage.Name, Justify.Left))
                    .BorderColor(ConvertStringToColor(consoleMessage.Color));
                AnsiConsole.Write(panel);
            }
            
        }

        return 0;
    }

    private static string DrawConsoleAndGetName(Settings settings)
    {
        var intro = new FigletText(FigletFont.Default, "Welcome to Console Chat!")
                    .Color(Color.Yellow)
                    .Centered();
        AnsiConsole.Write(intro);

        var channelInfo = new Rule($"You're subscribing to the {settings.Channel} channel.")
            .Centered();
        AnsiConsole.Write(channelInfo);

        var name = AnsiConsole.Ask<string>("What is your name?");
        return name;
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
