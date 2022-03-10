var app = new CommandApp();
app.Configure(config => 
{
    config.AddCommand<PublishCommand>("pub");
    config.AddCommand<SubscribeCommand>("sub");
});
return app.Run(args);