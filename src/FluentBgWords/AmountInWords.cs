namespace FluentBgWords;

using FluentBgWords.Internals;

/// <summary>
/// An amount configured for writing in Bulgarian words. Immutable: every method
/// returns a new instance, so a partially configured amount can be branched safely.
/// Call <see cref="ToString"/> to get the text.
/// </summary>
public readonly record struct AmountInWords
{
    internal AmountInWords(decimal amount)
    {
        this.Amount = AmountToWords.Validate(amount);
        this.SelectedCurrency = CurrencyDefinition.Eur;
    }

    private decimal Amount { get; init; }

    // Null only for default(AmountInWords), which bypasses the constructor; ToString() falls back to euro.
    private CurrencyDefinition? SelectedCurrency { get; init; }

    private AmountFormat Format { get; init; }

    private bool IsCapitalized { get; init; }

    /// <summary>Writes the amount in Bulgarian leva (BGN): "два лева и една стотинка".</summary>
    public AmountInWords AsBgn()
        => this with
        {
            SelectedCurrency = CurrencyDefinition.Bgn
        };

    /// <summary>Writes the amount in euro (EUR): "две евро и един цент". This is the default.</summary>
    public AmountInWords AsEur()
        => this with
        {
            SelectedCurrency = CurrencyDefinition.Eur
        };

    /// <summary>Writes the amount in euro with "евроцент" as the subunit: "пет евро и два евроцента".</summary>
    public AmountInWords AsEurWithEurocents()
        => this with
        {
            SelectedCurrency = CurrencyDefinition.EurWithEurocents
        };

    /// <summary>Writes the amount in a custom currency.</summary>
    /// <param name="currency">The currency to use.</param>
    /// <exception cref="ArgumentNullException"><paramref name="currency"/> is <see langword="null"/>.</exception>
    public AmountInWords As(CurrencyDefinition currency)
    {
        ArgumentNullException.ThrowIfNull(currency);
        return this with
        {
            SelectedCurrency = currency
        };
    }

    /// <summary>Writes the subunits as digits: "пет лева и 42 стотинки".</summary>
    public AmountInWords WithSubunitsAsDigits()
        => this with
        {
            Format = this.Format with
            {
                SubunitsAsDigits = true
            }
        };

    /// <summary>Capitalizes the first letter: "Пет лева".</summary>
    public AmountInWords Capitalized()
        => this with
        {
            IsCapitalized = true
        };

    /// <summary>Uses the currency abbreviations: "пет лв. и четиридесет и две ст.".</summary>
    public AmountInWords Abbreviated()
        => this with
        {
            Format = this.Format with
            {
                Abbreviated = true
            }
        };

    /// <summary>Returns the amount written in Bulgarian words.</summary>
    /// <remarks>
    /// This method does not throw: the amount is validated when the builder is created by
    /// <see cref="AmountInWordsExtensions.InWords(decimal)"/>.
    /// </remarks>
    public override string ToString()
    {
        var text = AmountToWords.Convert(
            this.Amount,
            this.SelectedCurrency ?? CurrencyDefinition.Eur, this.Format);

        return this.IsCapitalized
            ? char.ToUpperInvariant(text[0]) + text[1..]
            : text;
    }
}
