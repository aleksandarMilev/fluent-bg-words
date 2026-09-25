using System.Globalization;
using FluentBgWords.Internals;

namespace FluentBgWords.Tests;

public class AmountToWordsTests
{
    [Theory]
    [InlineData("0", "нула евро")]
    [InlineData("1", "едно евро")]
    [InlineData("2", "две евро")]
    [InlineData("21", "двадесет и едно евро")]
    [InlineData("1.01", "едно евро и един цент")]
    [InlineData("2.02", "две евро и два цента")]
    [InlineData("1.10", "едно евро и десет цента")]
    [InlineData("0.50", "нула евро и петдесет цента")]
    [InlineData("1.230", "едно евро и двадесет и три цента")]
    [InlineData("1234.56", "хиляда двеста тридесет и четири евро и петдесет и шест цента")]
    [InlineData("1000000", "един милион евро")]
    public void Convert_Eur(string amount, string expected)
        => Assert.Equal(
            expected,
            AmountToWords.Convert(Parse(amount), Currency.Eur));

    [Theory]
    [InlineData("1", "един лев")]
    [InlineData("2", "два лева")]
    [InlineData("21", "двадесет и един лева")]
    [InlineData("0.01", "нула лева и една стотинка")]
    [InlineData("0.02", "нула лева и две стотинки")]
    [InlineData("21.21", "двадесет и един лева и двадесет и една стотинки")]
    [InlineData("167.42", "сто шестдесет и седем лева и четиридесет и две стотинки")]
    [InlineData("1000", "хиляда лева")]
    [InlineData("2000000", "два милиона лева")]
    public void Convert_Bgn(string amount, string expected)
        => Assert.Equal(
            expected,
            AmountToWords.Convert(Parse(amount), Currency.Bgn));

    [Theory]
    [InlineData("167.42", "BGN", "сто шестдесет и седем лева и 42 стотинки")]
    [InlineData("0.01", "BGN", "нула лева и 1 стотинка")]
    [InlineData("21.21", "BGN", "двадесет и един лева и 21 стотинки")]
    [InlineData("1.05", "EUR", "едно евро и 5 цента")]
    [InlineData("1.01", "EUR", "едно евро и 1 цент")]
    [InlineData("5", "EUR", "пет евро")]
    [InlineData("-0.50", "EUR", "минус нула евро и 50 цента")]
    public void Convert_SubunitsAsDigits(
        string amount,
        string currencyCode,
        string expected)
        => Assert.Equal(
            expected,
            AmountToWords.Convert(
                Parse(amount),
                CurrencyFrom(currencyCode),
                subunitsAsDigits: true));

    [Fact]
    public void Convert_Negative_PrefixesMinus()
        => Assert.Equal(
            "минус пет евро",
            AmountToWords.Convert(-5m, Currency.Eur));

    [Fact]
    public void Convert_MoreThanTwoDecimals_Throws()
        => Assert.Throws<ArgumentException>(
            () => AmountToWords.Convert(1.234m, Currency.Eur));

    [Fact]
    public void Convert_AboveMaxValue_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(
            () => AmountToWords.Convert(1_000_000_000_000m, Currency.Eur));

    private static decimal Parse(string value)
        => decimal.Parse(value, CultureInfo.InvariantCulture);

    private static Currency CurrencyFrom(string code)
        => code switch
        {
            "BGN" => Currency.Bgn,
            "EUR" => Currency.Eur,
            _ => throw new ArgumentOutOfRangeException(
                nameof(code),
                code,
                "Unknown currency code in test data."),
        };
}
