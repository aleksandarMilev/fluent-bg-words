namespace FluentBgWords.Tests;

public class ReadmeExamplesTests
{
    // README: "Quick start"
    [Fact]
    public void AsEurWithEurocents_QuickStartExample_WritesEurocents()
        => Assert.Equal(
            "хиляда двеста тридесет и четири евро и петдесет и шест евроцента",
            1234.56m.InWords().AsEurWithEurocents().ToString());

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
    // README table row omits WithSubunitsAsDigits(); fixed in batch 4 (TEST-05 note).
    [Fact]
    public void Abbreviated_ApiTableExample_WritesAbbreviations()
        => Assert.Equal(
            "пет лв. и 42 ст.",
            5.42m.InWords().AsBgn().WithSubunitsAsDigits().Abbreviated().ToString());

    // README: "API" table, Capitalized() row
    [Fact]
    public void Capitalized_ApiTableExample_UppercasesFirstLetter()
        => Assert.Equal(
            "Пет лева",
            5m.InWords().AsBgn().Capitalized().ToString());
}
