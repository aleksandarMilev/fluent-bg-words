using FluentBgWords.Internals;

namespace FluentBgWords;

/// <summary>
/// An amount configured for writing in Bulgarian words. Immutable: every method
/// returns a new instance, so configurations can be safely shared and reused.
/// Call <see cref="ToString"/> to get the text.
/// </summary>
public readonly record struct AmountInWords
{
    internal AmountInWords(decimal amount)
        => this.Amount = amount;

    private decimal Amount { get; init; }

    private Currency? SelectedCurrency { get; init; }

    private AmountFormat Format { get; init; }

    private bool IsCapitalized { get; init; }

    /// <summary>Writes the amount in Bulgarian leva (BGN): "два лева и една стотинка".</summary>
    public AmountInWords AsBgn()
        => this with 
        { 
            SelectedCurrency = Currency.Bgn
        };

    /// <summary>Writes the amount in euro (EUR): "две евро и един цент". This is the default.</summary>
    public AmountInWords AsEur()
        => this with
        { 
            SelectedCurrency = Currency.Eur 
        };

    /// <summary>Writes the amount in a custom currency.</summary>
    /// <param name="currency">The currency to use.</param>
    public AmountInWords As(Currency currency)
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
            Format = Format with
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

    /// <summary>Uses the currency abbreviations: "пет лв. и 42 ст.".</summary>
    public AmountInWords Abbreviated()
        => this with 
        { 
            Format = Format with 
            { 
                Abbreviated = true 
            } 
        };

    /// <summary>Returns the amount written in Bulgarian words.</summary>
    /// <exception cref="ArgumentException">The amount has more than 2 decimal places.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The amount is outside the supported range.</exception>
    public override string ToString()
    {
        var text = AmountToWords.Convert(
            this.Amount,
            this.SelectedCurrency ?? Currency.Eur, this.Format);

        return this.IsCapitalized
            ? char.ToUpperInvariant(text[0]) + text[1..]
            : text;
    }
}