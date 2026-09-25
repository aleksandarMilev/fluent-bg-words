using System.Globalization;

namespace FluentBgWords.Tests;

public class PublicApiEdgeTests
{
    [Fact]
    public void As_Null_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => 5m.InWords().As(null!).ToString());

        Assert.Equal("currency", exception.ParamName);
    }

    [Fact]
    public void InWords_IntMinValue_PrefixesMinus()
        => Assert.Equal(
            "минус два милиарда сто четиридесет и седем милиона четиристотин осемдесет и три хиляди шестстотин четиридесет и осем евро",
            int.MinValue.InWords().ToString());

    [Fact]
    public void InWords_IntMaxValue_WritesWords()
        => Assert.Equal(
            "два милиарда сто четиридесет и седем милиона четиристотин осемдесет и три хиляди шестстотин четиридесет и седем евро",
            int.MaxValue.InWords().ToString());

    [Fact]
    public void InWords_LongMinValue_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(
            () => long.MinValue.InWords().ToString());

    [Fact]
    public void InWords_LongMaxValue_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(
            () => long.MaxValue.InWords().ToString());

    [Fact]
    public void InWords_MaxAmount_WritesWords()
        => Assert.Equal(
            "деветстотин деветдесет и девет милиарда деветстотин деветдесет и девет милиона деветстотин деветдесет и девет хиляди деветстотин деветдесет и девет лева и деветдесет и девет стотинки",
            999_999_999_999.99m.InWords().AsBgn().ToString());

    [Fact]
    public void InWords_NegativeMaxAmount_PrefixesMinus()
        => Assert.Equal(
            "минус деветстотин деветдесет и девет милиарда деветстотин деветдесет и девет милиона деветстотин деветдесет и девет хиляди деветстотин деветдесет и девет лева и деветдесет и девет стотинки",
            (-999_999_999_999.99m).InWords().AsBgn().ToString());

    [Theory]
    [InlineData("1000000000000")]
    [InlineData("-1000000000000")]
    public void InWords_JustAboveMaxAmount_Throws(string amount)
        => Assert.Throws<ArgumentOutOfRangeException>(
            () => Parse(amount).InWords().ToString());

    [Fact]
    public void InWords_DecimalMaxValue_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(
            () => decimal.MaxValue.InWords().ToString());

    [Fact]
    public void InWords_DecimalMinValue_Throws()
        => Assert.Throws<ArgumentOutOfRangeException>(
            () => decimal.MinValue.InWords().ToString());

    [Fact]
    public void InWords_ThreeDecimalsAboveMaxAmount_ThrowsArgumentException()
        => Assert.Throws<ArgumentException>(
            () => 999_999_999_999.991m.InWords().ToString());

    [Fact]
    public void InWords_NegativeZeroWithScale_WritesZeroWithoutMinus()
        => Assert.Equal(
            "нула евро",
            (-0.00m).InWords().ToString());

    [Fact]
    public void InWords_NegatedZero_WritesZeroWithoutMinus()
        => Assert.Equal(
            "нула евро",
            decimal.Negate(0m).InWords().ToString());

    [Theory]
    [InlineData("0.01", "нула лева и една стотинка")]
    [InlineData("0.10", "нула лева и десет стотинки")]
    [InlineData("0.99", "нула лева и деветдесет и девет стотинки")]
    [InlineData("-0.01", "минус нула лева и една стотинка")]
    public void InWords_SmallAmounts_WritesZeroLevaAndSubunits(
        string amount,
        string expected)
        => Assert.Equal(
            expected,
            Parse(amount).InWords().AsBgn().ToString());

    [Theory]
    [InlineData("1.2300", "един лев и двадесет и три стотинки")]
    [InlineData("1.00000000000000000000", "един лев")]
    public void InWords_TrailingZeroScale_IgnoresTrailingZeros(
        string amount,
        string expected)
        => Assert.Equal(
            expected,
            Parse(amount).InWords().AsBgn().ToString());

    [Fact]
    public void Default_AsBgn_WritesZeroLeva()
        => Assert.Equal(
            "нула лева",
            default(AmountInWords).AsBgn().ToString());

    [Fact]
    public void Default_Capitalized_WritesCapitalizedZeroEuro()
        => Assert.Equal(
            "Нула евро",
            default(AmountInWords).Capitalized().ToString());

    [Fact]
    public void Default_AbbreviatedWithDigits_WritesZeroWithAbbreviation()
        => Assert.Equal(
            "нула е.",
            default(AmountInWords).Abbreviated().WithSubunitsAsDigits().ToString());

    private static decimal Parse(string value)
        => decimal.Parse(value, CultureInfo.InvariantCulture);
}
