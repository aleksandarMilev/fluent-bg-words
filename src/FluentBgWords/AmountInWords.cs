namespace FluentBgWords;

using FluentBgWords.Internals;

/// <summary>
/// An amount configured for writing in Bulgarian words. Immutable: every method
/// returns a new instance, so a partially configured amount can be branched safely.
/// Call <see cref="ToString"/> to get the text.
/// </summary>
/// <remarks>
/// Create instances with <see cref="AmountInWordsExtensions.InWords(decimal)"/>.
/// <c>default(AmountInWords)</c> is a zero amount in euro and writes "нула евро".
/// </remarks>
public readonly record struct AmountInWords
{
    internal AmountInWords(decimal amount)
    {
        this.Amount = AmountToWords.Validate(amount);
        this.Settings = new(
            CurrencyDefinition.Eur,
            default);
    }

    private decimal Amount { get; init; }

    // Settings.Currency is null only for default(AmountInWords), which bypasses the constructor;
    // it is then written in euro.
    private AmountWordsSettings Settings { get; init; }

    /// <summary>Writes the amount in Bulgarian leva (BGN): "два лева и една стотинка".</summary>
    public AmountInWords AsBgn()
        => this.WithCurrency(CurrencyDefinition.Bgn);

    /// <summary>Writes the amount in euro (EUR): "две евро и един цент". This is the default.</summary>
    public AmountInWords AsEur()
        => this.WithCurrency(CurrencyDefinition.Eur);

    /// <summary>Writes the amount in euro with "евроцент" as the subunit: "пет евро и два евроцента".</summary>
    public AmountInWords AsEurWithEurocents()
        => this.WithCurrency(CurrencyDefinition.EurWithEurocents);

    /// <summary>Writes the amount in a custom currency.</summary>
    /// <param name="currency">The currency to use.</param>
    /// <exception cref="ArgumentNullException"><paramref name="currency"/> is <see langword="null"/>.</exception>
    public AmountInWords As(CurrencyDefinition currency)
    {
        ArgumentNullException.ThrowIfNull(currency);
        return this.WithCurrency(currency);
    }

    /// <summary>Writes the subunits as digits: "пет лева и 42 стотинки".</summary>
    public AmountInWords WithSubunitsAsDigits()
        => this.WithFormat(this.Settings.Format with
        {
            SubunitsAsDigits = true
        });

    /// <summary>Capitalizes the first letter: "Пет лева".</summary>
    public AmountInWords Capitalized()
        => this.WithFormat(this.Settings.Format with
        {
            Capitalized = true
        });

    /// <summary>Uses the currency abbreviations: "пет лв. и четиридесет и две ст.".</summary>
    public AmountInWords Abbreviated()
        => this.WithFormat(this.Settings.Format with
        {
            Abbreviated = true
        });

    /// <summary>Returns the amount written in Bulgarian words.</summary>
    /// <remarks>
    /// This method does not throw: the amount is validated when the builder is created by
    /// <see cref="AmountInWordsExtensions.InWords(decimal)"/>.
    /// </remarks>
    public override string ToString()
        => this.Settings.Write(this.Amount);

    private AmountInWords WithCurrency(CurrencyDefinition currency)
        => this with
        {
            Settings = this.Settings with
            {
                Currency = currency
            }
        };

    private AmountInWords WithFormat(AmountFormat format)
        => this with
        {
            Settings = this.Settings with
            {
                Format = format
            }
        };
}
