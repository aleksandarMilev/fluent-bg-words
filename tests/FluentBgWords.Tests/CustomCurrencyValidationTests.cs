namespace FluentBgWords.Tests;

public class CustomCurrencyValidationTests
{
    [Fact]
    public void Unit_NullSingular_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new Unit(null!, "лева", Gender.Masculine));

        Assert.Equal("Singular", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" лев")]
    [InlineData("лев ")]
    public void Unit_BlankOrPaddedSingular_ThrowsArgumentException(string singular)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new Unit(singular, "лева", Gender.Masculine));

        Assert.Equal("Singular", exception.ParamName);
    }

    [Fact]
    public void Unit_NullCountForm_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new Unit("лев", null!, Gender.Masculine));

        Assert.Equal("CountForm", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" лев")]
    [InlineData("лев ")]
    public void Unit_BlankOrPaddedCountForm_ThrowsArgumentException(string countForm)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new Unit("лев", countForm, Gender.Masculine));

        Assert.Equal("CountForm", exception.ParamName);
    }

    [Fact]
    public void Currency_NullMajor_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new Currency(
                null!,
                Currency.Bgn.Minor,
                "лв.",
                "ст."));

        Assert.Equal("Major", exception.ParamName);
    }

    [Fact]
    public void Currency_NullMinor_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new Currency(
                Currency.Bgn.Major,
                null!,
                "лв.",
                "ст."));

        Assert.Equal("Minor", exception.ParamName);
    }

    [Fact]
    public void Currency_NullMajorAbbreviation_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new Currency(
                Currency.Bgn.Major,
                Currency.Bgn.Minor,
                null!,
                "ст."));

        Assert.Equal("MajorAbbreviation", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" лв.")]
    public void Currency_BlankOrPaddedMajorAbbreviation_ThrowsArgumentException(string abbreviation)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new Currency(
                Currency.Bgn.Major,
                Currency.Bgn.Minor,
                abbreviation,
                "ст."));

        Assert.Equal("MajorAbbreviation", exception.ParamName);
    }

    [Fact]
    public void Currency_NullMinorAbbreviation_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new Currency(
                Currency.Bgn.Major,
                Currency.Bgn.Minor,
                "лв.",
                null!));

        Assert.Equal("MinorAbbreviation", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" лв.")]
    public void Currency_BlankOrPaddedMinorAbbreviation_ThrowsArgumentException(string abbreviation)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new Currency(
                Currency.Bgn.Major,
                Currency.Bgn.Minor,
                "лв.",
                abbreviation));

        Assert.Equal("MinorAbbreviation", exception.ParamName);
    }

    [Fact]
    public void Currency_WithEmptyMajorAbbreviation_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(
            () => Currency.Bgn with { MajorAbbreviation = "" });

        Assert.Equal("MajorAbbreviation", exception.ParamName);
    }

    [Fact]
    public void Currency_WithNullMinor_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => Currency.Eur with { Minor = null! });

        Assert.Equal("Minor", exception.ParamName);
    }

    [Fact]
    public void Unit_WithWhiteSpaceSingular_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(
            () => Currency.Bgn.Major with { Singular = " " });

        Assert.Equal("Singular", exception.ParamName);
    }

    [Fact]
    public void As_ValidCustomCurrency_WritesAmount()
    {
        var sek = new Currency(
            new Unit("крона", "крони", Gender.Feminine),
            new Unit("йоре", "йоре", Gender.Neuter),
            "кр.",
            "й.");

        Assert.Equal(
            "двадесет и една крони и две йоре",
            21.02m.InWords().As(sek).ToString());
    }

    [Fact]
    public void Unit_EqualValues_AreEqual()
        => Assert.Equal(
            new Unit("лев", "лева", Gender.Masculine),
            new Unit("лев", "лева", Gender.Masculine));

    [Fact]
    public void Currency_EqualValues_AreEqual()
        => Assert.Equal(
            Currency.Bgn,
            new Currency(
                new Unit("лев", "лева", Gender.Masculine),
                new Unit("стотинка", "стотинки", Gender.Feminine),
                "лв.",
                "ст."));
}
