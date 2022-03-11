public sealed class PublishCommand : AsyncCommand<Settings>
{
    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var ably = new AblyRealtime(settings.AblyApiKey);
        var channel = ably.Channels.Get(settings.Channel);
        (string name, string color) input = DrawConsoleAndGetInput(settings);

        while (true)
        {
            var text = AnsiConsole.Ask<string>($"[{input.color}]{input.name}: [/]");
            var chatMessage = new ChatMessage(input.name, text, input.color);

            await channel.PublishAsync(nameof(ChatMessage), chatMessage);
        }

        return 0;
    }

    private static (string name, string color) DrawConsoleAndGetInput(Settings settings)
    {
        var intro = new FigletText(FigletFont.Default, "Welcome to Console Chat!").Color(Color.Yellow).Centered();
        AnsiConsole.Write(intro);

        var channelInfo = new Rule($"You're publishing to the {settings.Channel} channel.")
            .Centered();
        AnsiConsole.Write(channelInfo);

        var name = AnsiConsole.Ask<string>("What is your name?");
        var color = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a color for your messages:")
                .AddChoices(new[] {
                    "red",
                    "green",
                    "blue"
        }));

        return (name, color);
    }
}
