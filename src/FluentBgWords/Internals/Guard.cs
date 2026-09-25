namespace FluentBgWords.Internals;

internal static class Guard
{
    public static string AgainstLeadingOrTrailingWhitespace(
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

    public static T AgainstNull<T>(
        T value,
        string paramName)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
        return value;
    }

    public static TEnum AgainstUndefinedEnum<TEnum>(
        TEnum value,
        string paramName)
        where TEnum : struct, Enum
    {
        if (!Enum.IsDefined(value))
        {
            throw new ArgumentOutOfRangeException(
                paramName,
                value,
                $"Value is not a defined {typeof(TEnum).Name} member.");
        }

        return value;
    }
}
