public sealed class ChatMessage
{
    public ChatMessage(string name, string message, string color)
    {
        Name = name;
        Message = message;
        Color = color;
    }
    public string Name { get; set; }
    public string Message { get; set; }
    public string Color { get; set; }
}
