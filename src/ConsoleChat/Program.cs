var app = new CommandApp();
app.Configure(config =>
{
    config.SetApplicationName("ConsoleChat.exe");
    config.AddCommand<PublishCommand>("pub");
    config.AddCommand<SubscribeCommand>("sub");
    config.AddExample(new[] { "pub", "channel1", "AblyApiKey" });
    config.AddExample(new[] { "sub", "channel1", "AblyApiKey" });
});
return app.Run(args);
