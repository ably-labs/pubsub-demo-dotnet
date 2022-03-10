public sealed class Settings : CommandSettings
{
    [CommandArgument(0, "<channel>")]
    public string Channel { get; set; }
    [CommandArgument(1, "<connection>")]
    public string AblyConnection { get; set; }
}
