public sealed class ChatCommand : AsyncCommand<ChatCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "<name>")]
        public string Name { get; set; }

        [CommandArgument(1, "<channel>")]
        public string Channel { get; set; }

        [CommandArgument(2, "<connection>")]
        public string AblyConnection { get; set; }
    }

    public sealed class ChatMessage
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }

    public override Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var ably = new AblyRealtime(settings.AblyConnection);
        ably.Connection.On(ConnectionEvent.Connected, args =>
        {
            AnsiConsole.MarkupLine("Connected to Ably!");
        });
        var channel = ably.Channels.Get(settings.Channel);
        channel.Subscribe(message =>
        {
            var chatMessage = (ChatMessage)message.Data;
            AnsiConsole.MarkupLine($"{chatMessage.Name}: {chatMessage.Message}");
        });

        var introRule = new Rule("[red]Welcome to Console Chat![/]");
        AnsiConsole.Write(introRule);
        // var connection =  AnsiConsole.Prompt(
        //     new TextPrompt<string>("1/3 - Paste the Ably [green]connection string[/]:")
        //     .Secret());
        // var channel = AnsiConsole.Ask<string>("2/3 - Enter the [green]channel name[/]:");
        // var name = AnsiConsole.Ask<string>("3/3 - Enter your [green]name[/]:");

        AnsiConsole.MarkupLine($"!");
        var chatRule = new Rule($"[green]Hi {settings.Name}, this is the {settings.Channel} channel. Let's start typing![/]");
        AnsiConsole.Write(chatRule);
        

        string input = string.Empty;
        while (input.ToLower() != "x" ) {
            input = AnsiConsole.Ask<string>(">:");
            var chatMessage = new ChatMessage { Name = settings.Name, Message = input };
            channel.PublishAsync("chat", chatMessage);
        }

        return Task.FromResult(0);
    }
}