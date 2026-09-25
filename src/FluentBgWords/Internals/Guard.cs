namespace FluentBgWords.Internals;

internal static class Guard
{
    public static string Text(
        string value,
        string paramName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, paramName);

        if (char.IsWhiteSpace(value[0]) || char.IsWhiteSpace(value[^1]))
        {
            throw new ArgumentException(
                "Value must not have leading or trailing whitespace.",
                paramName);
        }

        return value;
    }

    public static T NotNull<T>(
        T value,
        string paramName)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
        return value;
    }
}
