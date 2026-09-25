namespace FluentBgWords.Tests;

public class ReadmeExamplesTests
{
    // README: "Quick start"
    [Fact]
    public void AsEurWithEurocents_QuickStartExample_WritesEurocents()
        => Assert.Equal(
            "хиляда двеста тридесет и четири евро и петдесет и шест евроцента",
            1234.56m.InWords().AsEurWithEurocents().ToString());

    // README: "API" table, InWords() row
    [Fact]
    public void InWords_ApiTableExample_DefaultsToEuro()
        => Assert.Equal(
            "пет евро",
            5m.InWords().ToString());

    // README: "API" table, AsEur() row
    [Fact]
    public void AsEur_ApiTableExample_WritesCents()
        => Assert.Equal(
            "две евро и два цента",
            2.02m.InWords().AsEur().ToString());

    // README: "API" table, AsBgn() row
    [Fact]
    public void AsBgn_ApiTableExample_WritesLevaAndStotinki()
        => Assert.Equal(
            "два лева и две стотинки",
            2.02m.InWords().AsBgn().ToString());

    // README: "API" table, WithSubunitsAsDigits() row
    [Fact]
    public void WithSubunitsAsDigits_ApiTableExample_WritesDigits()
        => Assert.Equal(
            "пет лева и 42 стотинки",
            5.42m.InWords().AsBgn().WithSubunitsAsDigits().ToString());

    // README: "API" table, Abbreviated() row
    [Fact]
    public void Abbreviated_ApiTableExample_WritesAbbreviations()
        => Assert.Equal(
            "пет лв. и четиридесет и две ст.",
            5.42m.InWords().AsBgn().Abbreviated().ToString());

    // README: "Behavior", nullable amounts
    [Fact]
    public void InWords_NullAmount_ReturnsNull()
    {
        decimal? amount = null;

        Assert.Null(amount?.InWords().AsBgn().ToString());
    }

    // README: "Behavior", nullable amounts
    [Fact]
    public void InWords_NullableAmountWithValue_WritesWords()
    {
        decimal? amount = 5m;

        Assert.Equal(
            "пет лева",
            amount?.InWords().AsBgn().ToString());
    }

    // README: "API" table, Capitalized() row
    [Fact]
    public void Capitalized_ApiTableExample_UppercasesFirstLetter()
        => Assert.Equal(
            "Пет лева",
            5m.InWords().AsBgn().Capitalized().ToString());
}
