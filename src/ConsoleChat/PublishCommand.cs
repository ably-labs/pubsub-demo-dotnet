public sealed class PublishCommand : AsyncCommand<Settings>
{
    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        (string Name, string Color) input = DrawConsoleAndGetInput(settings);

        var clientOptions = new ClientOptions(settings.AblyApiKey) { ClientId = input.Name };
        var ably = new AblyRealtime(clientOptions);
        var channel = ably.Channels.Get(settings.Channel);
        channel.Presence.Enter(input.Color);

        while (true)
        {
            var text = AnsiConsole.Ask<string>($"[{input.Color}]{input.Name }: [/]");
            var result  = await channel.PublishAsync("chat", text);
            if (result.IsFailure)
            {
                AnsiConsole.MarkupLine($"[red]{result.Error.Message}[/]");
            }
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
