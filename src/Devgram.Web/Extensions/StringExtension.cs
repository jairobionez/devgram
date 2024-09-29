namespace Devgram.Web.Extensions;

public static class StringExtension
{
    public static string Truncate(this string value, int limit)
    {
        return value.Length > limit ? value.Substring(0, limit) + "..." : value;
    }
}