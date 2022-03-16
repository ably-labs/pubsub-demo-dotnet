public class Program
{
    public static async Task<int> Main(string[] args)
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.AddCommand<PublishCommand>("pub")
                .WithDescription("Publish messages to a channel.")
                .WithExample(new[] { "pub", "channel1", "AblyApiKey" });
            config.AddCommand<SubscribeCommand>("sub")
                .WithDescription("Subscribe to a channel.")
                .WithExample(new[] { "sub", "channel1", "AblyApiKey" });
            config.SetApplicationName("ConsoleChat.exe");
        });

        return await app.RunAsync(args);
    }
}
