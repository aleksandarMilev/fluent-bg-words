namespace FluentBgWords.Tests;

public class FluentApiTests
{
    [Fact]
    public void DefaultsToEuro()
        => Assert.Equal(
            "хиляда двеста тридесет и четири евро и петдесет и шест цента",
            1234.56m.InWords().ToString());

    [Fact]
    public void AsBgn_UsesLeva()
        => Assert.Equal(
            "двадесет и един лева и една стотинка",
            21.01m.InWords().AsBgn().ToString());

    [Fact]
    public void WithSubunitsAsDigits_WritesDigits()
        => Assert.Equal(
            "сто шестдесет и седем лева и 42 стотинки",
            167.42m.InWords().AsBgn().WithSubunitsAsDigits().ToString());

    [Fact]
    public void Capitalized_UppercasesFirstLetter()
        => Assert.Equal("Пет евро", 5m.InWords().Capitalized().ToString());

    [Fact]
    public void FullChain()
        => Assert.Equal(
            "Сто шестдесет и седем лева и 42 стотинки",
            167.42m.InWords().AsBgn().WithSubunitsAsDigits().Capitalized().ToString());

    [Fact]
    public void Builder_IsImmutable()
    {
        var bgn = 5m.InWords().AsBgn();
        _ = bgn.Capitalized();

        Assert.Equal("пет лева", bgn.ToString());
    }

    [Fact]
    public void Default_WritesZeroEuro()
        => Assert.Equal(
            "нула евро",
            default(AmountInWords).ToString());

    [Fact]
    public void As_CustomCurrency()
    {
        var usd = new Currency(
            new Unit("долар", "долара", Gender.Masculine),
            new Unit("цент", "цента", Gender.Masculine),
            "$",
            "ц.");

        Assert.Equal(
            "два долара и един цент",
            2.01m.InWords().As(usd).ToString());
    }

    [Fact]
    public void Abbreviated_ClassicInvoiceFormat()
        => Assert.Equal(
            "Сто шестдесет и седем лв. и 42 ст.",
            167.42m.InWords().AsBgn().WithSubunitsAsDigits().Abbreviated().Capitalized().ToString());

    [Fact]
    public void InWords_Int()
        => Assert.Equal(
            "пет лева",
            5.InWords().AsBgn().ToString());

    [Fact]
    public void InWords_Long()
        => Assert.Equal(
            "два милиарда евро",
            2_000_000_000L.InWords().ToString());

    [Fact]
    public void AsEurWithEurocents_AbbreviatedInvoiceFormat()
    => Assert.Equal(
        "Едно е. и 5 е.ц.",
        1.05m.InWords().AsEurWithEurocents().WithSubunitsAsDigits().Abbreviated().Capitalized().ToString());
}