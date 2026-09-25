namespace FluentBgWords.Internals;

using FluentBgWords;
using System.Globalization;

internal static class AmountToWords
{
    private const decimal MaxAmount = NumberToWords.MaxValue + 0.99m;

    public static string Convert(
        decimal amount,
        Currency currency,
        bool subunitsAsDigits = false)
    {
        ArgumentNullException.ThrowIfNull(currency);

        if (decimal.Round(amount, 2) != amount)
        {
            throw new ArgumentException(
                "Amount must have at most 2 decimal places. Round it before converting.",
                nameof(amount));
        }

        ArgumentOutOfRangeException.ThrowIfGreaterThan(
            Math.Abs(amount),
            MaxAmount,
            nameof(amount));

        var prefix = amount < 0 ? "минус " : string.Empty;
        amount = Math.Abs(amount);

        var major = (long)decimal.Truncate(amount);
        var minor = (int)((amount - major) * 100);

        var result = $"{NumberToWords.Convert(major, currency.Major.Gender)} {currency.Major.FormFor(major)}";

        if (minor > 0)
        {
            var minorText = subunitsAsDigits
                ? minor.ToString(CultureInfo.InvariantCulture)
                : NumberToWords.Convert(minor, currency.Minor.Gender);

            result += $" и {minorText} {currency.Minor.FormFor(minor)}";
        }

        return prefix + result;
    }
}
