namespace FluentBgWords.Tests;

public class CustomCurrencyValidationTests
{
    [Fact]
    public void CurrencyUnit_NullSingular_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new CurrencyUnit(null!, "лева", GrammaticalGender.Masculine));

        Assert.Equal("Singular", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" лев")]
    [InlineData("лев ")]
    public void CurrencyUnit_BlankOrPaddedSingular_ThrowsArgumentException(string singular)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new CurrencyUnit(singular, "лева", GrammaticalGender.Masculine));

        Assert.Equal("Singular", exception.ParamName);
    }

    [Fact]
    public void CurrencyUnit_NullCountForm_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new CurrencyUnit("лев", null!, GrammaticalGender.Masculine));

        Assert.Equal("CountForm", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" лев")]
    [InlineData("лев ")]
    public void CurrencyUnit_BlankOrPaddedCountForm_ThrowsArgumentException(string countForm)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new CurrencyUnit("лев", countForm, GrammaticalGender.Masculine));

        Assert.Equal("CountForm", exception.ParamName);
    }

    [Fact]
    public void CurrencyDefinition_NullMajor_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new CurrencyDefinition(
                null!,
                CurrencyDefinition.Bgn.Minor,
                "лв.",
                "ст."));

        Assert.Equal("Major", exception.ParamName);
    }

    [Fact]
    public void CurrencyDefinition_NullMinor_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new CurrencyDefinition(
                CurrencyDefinition.Bgn.Major,
                null!,
                "лв.",
                "ст."));

        Assert.Equal("Minor", exception.ParamName);
    }

    [Fact]
    public void CurrencyDefinition_NullMajorAbbreviation_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new CurrencyDefinition(
                CurrencyDefinition.Bgn.Major,
                CurrencyDefinition.Bgn.Minor,
                null!,
                "ст."));

        Assert.Equal("MajorAbbreviation", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" лв.")]
    public void CurrencyDefinition_BlankOrPaddedMajorAbbreviation_ThrowsArgumentException(string abbreviation)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new CurrencyDefinition(
                CurrencyDefinition.Bgn.Major,
                CurrencyDefinition.Bgn.Minor,
                abbreviation,
                "ст."));

        Assert.Equal("MajorAbbreviation", exception.ParamName);
    }

    [Fact]
    public void CurrencyDefinition_NullMinorAbbreviation_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new CurrencyDefinition(
                CurrencyDefinition.Bgn.Major,
                CurrencyDefinition.Bgn.Minor,
                "лв.",
                null!));

        Assert.Equal("MinorAbbreviation", exception.ParamName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(" лв.")]
    public void CurrencyDefinition_BlankOrPaddedMinorAbbreviation_ThrowsArgumentException(string abbreviation)
    {
        var exception = Assert.Throws<ArgumentException>(
            () => new CurrencyDefinition(
                CurrencyDefinition.Bgn.Major,
                CurrencyDefinition.Bgn.Minor,
                "лв.",
                abbreviation));

        Assert.Equal("MinorAbbreviation", exception.ParamName);
    }

    [Fact]
    public void CurrencyDefinition_WithEmptyMajorAbbreviation_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(
            () => CurrencyDefinition.Bgn with { MajorAbbreviation = "" });

        Assert.Equal("MajorAbbreviation", exception.ParamName);
    }

    [Fact]
    public void CurrencyDefinition_WithNullMinor_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => CurrencyDefinition.Eur with { Minor = null! });

        Assert.Equal("Minor", exception.ParamName);
    }

    [Fact]
    public void CurrencyUnit_WithWhiteSpaceSingular_ThrowsArgumentException()
    {
        var exception = Assert.Throws<ArgumentException>(
            () => CurrencyDefinition.Bgn.Major with { Singular = " " });

        Assert.Equal("Singular", exception.ParamName);
    }

    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    public void CurrencyUnit_UndefinedGender_ThrowsArgumentOutOfRangeException(int gender)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new CurrencyUnit("лев", "лева", (GrammaticalGender)gender));

        Assert.Equal("Gender", exception.ParamName);
    }

    [Theory]
    [InlineData(42)]
    [InlineData(-1)]
    public void CurrencyUnit_WithUndefinedGender_ThrowsArgumentOutOfRangeException(int gender)
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => CurrencyDefinition.Bgn.Major with { Gender = (GrammaticalGender)gender });

        Assert.Equal("Gender", exception.ParamName);
    }

    [Fact]
    public void As_ValidCustomCurrency_WritesAmount()
    {
        var sek = new CurrencyDefinition(
            new CurrencyUnit("крона", "крони", GrammaticalGender.Feminine),
            new CurrencyUnit("йоре", "йоре", GrammaticalGender.Neuter),
            "кр.",
            "й.");

        Assert.Equal(
            "двадесет и една крони и две йоре",
            21.02m.InWords().As(sek).ToString());
    }

    [Fact]
    public void CurrencyUnit_EqualValues_AreEqual()
        => Assert.Equal(
            new CurrencyUnit("лев", "лева", GrammaticalGender.Masculine),
            new CurrencyUnit("лев", "лева", GrammaticalGender.Masculine));

    [Fact]
    public void CurrencyDefinition_EqualValues_AreEqual()
        => Assert.Equal(
            CurrencyDefinition.Bgn,
            new CurrencyDefinition(
                new CurrencyUnit("лев", "лева", GrammaticalGender.Masculine),
                new CurrencyUnit("стотинка", "стотинки", GrammaticalGender.Feminine),
                "лв.",
                "ст."));
}
