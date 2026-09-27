namespace FluentBgWords.Tests;

using System.Globalization;

public class AmountWordsFormatterTests
{
    // The amounts and currency codes of FormatMatrixTests, plus zero and a two-digit subunit.
    private static readonly string[] Amounts = ["1.01", "-2.02", "-0.05", "0", "21.21"];

    // SEK is the custom currency of CustomCurrencyValidationTests.As_ValidCustomCurrency_WritesAmount.
    private static readonly string[] Currencies = ["EUR", "EUR_CENTS", "BGN", "SEK"];

    private static readonly CurrencyDefinition Sek = new(
        new CurrencyUnit("крона", "крони", GrammaticalGender.Feminine),
        new CurrencyUnit("йоре", "йоре", GrammaticalGender.Neuter),
        "кр.",
        "й.");

    public static TheoryData<string, string, bool, bool, bool> FormatMatrix()
    {
        var data = new TheoryData<string, string, bool, bool, bool>();
        bool[] flags = [false, true];

        foreach (var amount in Amounts)
        {
            foreach (var currency in Currencies)
            {
                foreach (var digits in flags)
                {
                    foreach (var abbreviated in flags)
                    {
                        foreach (var capitalized in flags)
                        {
                            data.Add(amount, currency, digits, abbreviated, capitalized);
                        }
                    }
                }
            }
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(FormatMatrix))]
    public void Format_AnyCurrencyAndOptions_MatchesFluentBuilder(
        string amount,
        string currency,
        bool digits,
        bool abbreviated,
        bool capitalized)
    {
        var value = Parse(amount);
        var words = value.InWords().As(CurrencyFrom(currency));

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

        var formatter = new AmountWordsFormatter(new AmountWordsOptions
        {
            Currency = CurrencyFrom(currency),
            SubunitsAsDigits = digits,
            Abbreviated = abbreviated,
            Capitalized = capitalized,
        });

        Assert.Equal(
            words.ToString(),
            formatter.Format(value));
    }

    [Fact]
    public void Format_DefaultConstructor_WritesEuro()
        => Assert.Equal(
            "пет евро",
            new AmountWordsFormatter().Format(5m));

    [Fact]
    public void Options_Defaults_AreEuroWithoutOptions()
    {
        var options = new AmountWordsOptions();

        Assert.Equal(CurrencyDefinition.Eur, options.Currency);
        Assert.False(options.SubunitsAsDigits);
        Assert.False(options.Abbreviated);
        Assert.False(options.Capitalized);
    }

    [Fact]
    public void Format_OptionsChangedAfterConstruction_KeepsOriginalSettings()
    {
        var options = new AmountWordsOptions
        {
            Currency = CurrencyDefinition.Bgn,
            SubunitsAsDigits = true,
            Abbreviated = true,
        };
        var formatter = new AmountWordsFormatter(options);

        options.Currency = CurrencyDefinition.EurWithEurocents;
        options.SubunitsAsDigits = false;
        options.Abbreviated = false;
        options.Capitalized = true;

        Assert.Equal(
            167.42m.InWords().AsBgn().WithSubunitsAsDigits().Abbreviated().ToString(),
            formatter.Format(167.42m));
    }

    [Fact]
    public void Constructor_NullOptions_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new AmountWordsFormatter(null!));

        Assert.Equal("options", exception.ParamName);
    }

    [Fact]
    public void Constructor_NullCurrency_ThrowsArgumentNullException()
    {
        var exception = Assert.Throws<ArgumentNullException>(
            () => new AmountWordsFormatter(new AmountWordsOptions
            {
                Currency = null!,
            }));

        Assert.Equal("options", exception.ParamName);
        Assert.Equal(
            "AmountWordsOptions.Currency must not be null. Set it to a CurrencyDefinition, such as CurrencyDefinition.Eur. (Parameter 'options')",
            exception.Message);
    }

    [Theory]
    [InlineData("1.234")]
    [InlineData("0.001")]
    [InlineData("999999999999.991")]
    public void Format_MoreThanTwoDecimals_ThrowsLikeInWords(string amount)
    {
        var value = Parse(amount);
        var formatter = new AmountWordsFormatter();

        var expected = Assert.Throws<ArgumentException>(() => value.InWords());
        var actual = Assert.Throws<ArgumentException>(() => formatter.Format(value));

        Assert.Equal("amount", actual.ParamName);
        Assert.Equal(expected.ParamName, actual.ParamName);
        Assert.Equal(expected.Message, actual.Message);
    }

    [Theory]
    [InlineData("1000000000000")]
    [InlineData("-1000000000000")]
    [InlineData("79228162514264337593543950335")]
    [InlineData("-79228162514264337593543950335")]
    public void Format_OutOfRange_ThrowsLikeInWords(string amount)
    {
        var value = Parse(amount);
        var formatter = new AmountWordsFormatter();

        var expected = Assert.Throws<ArgumentOutOfRangeException>(() => value.InWords());
        var actual = Assert.Throws<ArgumentOutOfRangeException>(() => formatter.Format(value));

        Assert.Equal("amount", actual.ParamName);
        Assert.Equal(value, actual.ActualValue);
        Assert.Equal(expected.ParamName, actual.ParamName);
        Assert.Equal(expected.ActualValue, actual.ActualValue);
    }

    [Fact]
    public void Format_ThroughInterface_WritesSameText()
    {
        // The explicit interface type is the point of this test: keep it, don't change it to var.
        IAmountWordsFormatter formatter = new AmountWordsFormatter(new AmountWordsOptions
        {
            Currency = CurrencyDefinition.Bgn,
        });

        Assert.Equal(
            21.01m.InWords().AsBgn().ToString(),
            formatter.Format(21.01m));
    }

    private static decimal Parse(string value)
        => decimal.Parse(value, CultureInfo.InvariantCulture);

    private static CurrencyDefinition CurrencyFrom(string code)
        => code switch
        {
            "EUR" => CurrencyDefinition.Eur,
            "EUR_CENTS" => CurrencyDefinition.EurWithEurocents,
            "BGN" => CurrencyDefinition.Bgn,
            "SEK" => Sek,
            _ => throw new ArgumentOutOfRangeException(
                nameof(code),
                code,
                "Unknown currency code in test data."),
        };
}
