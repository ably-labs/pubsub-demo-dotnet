public static class JTokenObjectExtensions
{
    public static T ToObject<T>(this object jTokenObject)
    {
        return ((JToken)jTokenObject).ToObject<T>();
    }
}
