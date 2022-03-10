var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<PublishCommand>("pub");
    config.AddCommand<SubscribeCommand>("sub");
    config.SetApplicationName("ConsoleChat.exe");
    config.AddExample(new[] { "pub", "channel1", "AblyApiKey" });
    config.AddExample(new[] { "sub", "channel1", "AblyApiKey" });
});
return app.Run(args);
