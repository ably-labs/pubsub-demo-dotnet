public sealed class Settings : CommandSettings
{
    public Settings(string channel, string ablyApiKey)
    {
        Channel = channel;
        AblyApiKey = ablyApiKey;
    }
    
    [CommandArgument(0, "<channel>")]
    public string Channel { get; }
    [CommandArgument(1, "<connection>")]
    public string AblyApiKey { get; }
}
