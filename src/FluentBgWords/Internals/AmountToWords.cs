namespace FluentBgWords.Internals;

using System.Globalization;
using FluentBgWords;

internal static class AmountToWords
{
    private const decimal MaxAmount = NumberToWords.MaxValue + 0.99m;

    public static decimal Validate(decimal amount)
    {
        if (decimal.Round(amount, 2) != amount)
        {
            throw new ArgumentException(
                "Amount must have at most 2 decimal places. Round it before converting.",
                nameof(amount));
        }

        ArgumentOutOfRangeException.ThrowIfGreaterThan(
            amount,
            MaxAmount,
            nameof(amount));

        ArgumentOutOfRangeException.ThrowIfLessThan(
            amount,
            -MaxAmount,
            nameof(amount));

        return amount;
    }

    public static string Convert(
        decimal amount,
        CurrencyDefinition currency,
        AmountFormat format = default)
    {
        ArgumentNullException.ThrowIfNull(currency);
        Validate(amount);

        var prefix = amount < 0 ? "минус " : string.Empty;
        amount = Math.Abs(amount);

        var major = (long)decimal.Truncate(amount);
        var minor = (int)((amount - major) * 100);

        var majorUnit = format.Abbreviated
                ? currency.MajorAbbreviation
                : currency.Major.FormFor(major);

        var result = $"{NumberToWords.Convert(major, currency.Major.Gender)} {majorUnit}";

        if (minor > 0)
        {
            var minorText = format.SubunitsAsDigits
                ? minor.ToString(CultureInfo.InvariantCulture)
                : NumberToWords.Convert(minor, currency.Minor.Gender);

            var minorUnit = format.Abbreviated
                ? currency.MinorAbbreviation
                : currency.Minor.FormFor(minor);

            result += $" и {minorText} {minorUnit}";
        }

        var text = prefix + result;

        return format.Capitalized
            ? char.ToUpperInvariant(text[0]) + text[1..]
            : text;
    }
}
