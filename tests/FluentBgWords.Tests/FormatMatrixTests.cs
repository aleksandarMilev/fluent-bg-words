using System.Globalization;

namespace FluentBgWords.Tests;

public class FormatMatrixTests
{
    // Spelled-out numerals next to abbreviations ("един ц.", "една ст.") pin current behavior.
    // NEEDS NATIVE VERIFICATION, see CODE_REVIEW.md TEST-03.
    [Theory]
    [InlineData("1.01", "EUR", false, true, true, "Едно е. и един ц.")]
    [InlineData("1.01", "EUR", true, true, true, "Едно е. и 1 ц.")]
    [InlineData("1.01", "EUR", true, false, true, "Едно евро и 1 цент")]
    [InlineData("-2.02", "EUR", false, false, true, "Минус две евро и два цента")]
    [InlineData("-2.02", "EUR", true, true, true, "Минус две е. и 2 ц.")]
    [InlineData("1.01", "EUR_CENTS", false, true, true, "Едно е. и един е.ц.")]
    [InlineData("-0.05", "EUR_CENTS", false, false, true, "Минус нула евро и пет евроцента")]
    [InlineData("1.01", "BGN", false, true, true, "Един лв. и една ст.")]
    [InlineData("-2.02", "BGN", false, true, true, "Минус два лв. и две ст.")]
    [InlineData("-0.05", "BGN", true, true, true, "Минус нула лв. и 5 ст.")]
    public void InWords_CurrencyAndFormat_WritesExpectedText(
        string amount,
        string currency,
        bool digits,
        bool abbreviated,
        bool capitalized,
        string expected)
    {
        var words = decimal
            .Parse(amount, CultureInfo.InvariantCulture)
            .InWords()
            .As(CurrencyFrom(currency));

        if (digits)
        {
            words = words.WithSubunitsAsDigits();
        }

        if (abbreviated)
        {
            words = words.Abbreviated();
        }

        if (capitalized)
        {
            words = words.Capitalized();
        }

        Assert.Equal(
            expected,
            words.ToString());
    }

    private static Currency CurrencyFrom(string code)
        => code switch
        {
            "EUR" => Currency.Eur,
            "EUR_CENTS" => Currency.EurWithEurocents,
            "BGN" => Currency.Bgn,
            _ => throw new ArgumentOutOfRangeException(
                nameof(code),
                code,
                "Unknown currency code in test data."),
        };
}
