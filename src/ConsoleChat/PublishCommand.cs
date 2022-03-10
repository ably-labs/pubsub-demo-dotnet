public sealed class PublishCommand : AsyncCommand<Settings>
{
    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var ably = new AblyRealtime(settings.AblyConnection);
        var channel = ably.Channels.Get(
            settings.Channel, 
            new ChannelOptions { 
                Params = new ChannelParams {{ "rewind", "2m" }}
            });

        var welcomeRule = new Rule("[red]Welcome to Console Chat![/]").Border(BoxBorder.Double);
        welcomeRule.Alignment = Justify.Left;
        AnsiConsole.Write(welcomeRule);

        var channelRule = new Rule($"You're publishing to the {settings.Channel} channel.");
        channelRule.Alignment = Justify.Left;
        AnsiConsole.Write(channelRule);

        var name = AnsiConsole.Ask<string>("What is your name?");
        var color = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a color for your messages:")
                .AddChoices(new[] {
                    "red",
                    "green",
                    "blue"
                }));

        while (true) {
            var text = AnsiConsole.Ask<string>($"[{color}]{name}: [/]");
            var chatMessage = new ChatMessage(name, text, color);

            await channel.PublishAsync(nameof(ChatMessage), chatMessage);
        }

        return 0;
    }
}