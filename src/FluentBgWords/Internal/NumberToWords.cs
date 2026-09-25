namespace FluentBgWords.Internal;

internal static class NumberToWords
{
    public const long MaxValue = 999_999_999_999;

    public static string Convert(
        long number,
        Gender gender = Gender.Masculine)
        => throw new NotImplementedException($"{nameof(NumberToWords)}.{nameof(NumberToWords.Convert)} is not implemented!");
}
