namespace FluentBgWords;

/// <summary>
/// Settings for an <see cref="AmountWordsFormatter"/>: the currency and the formatting options.
/// Mutable, so it works with the .NET options pattern
/// (<c>services.Configure&lt;AmountWordsOptions&gt;(…)</c>). Set <see cref="Currency"/> in code:
/// binding it from configuration by name (for example <c>"Bgn"</c>) is not supported, and such a
/// value is ignored, so the currency stays euro.
/// </summary>
/// <remarks>
/// <see cref="AmountWordsFormatter"/> copies these settings when it is created. Changing this object
/// afterwards does not affect a formatter that was already created from it.
/// </remarks>
public sealed class AmountWordsOptions
{
    /// <summary>The currency to write amounts in. Defaults to <see cref="CurrencyDefinition.Eur"/>.</summary>
    public CurrencyDefinition Currency { get; set; } = CurrencyDefinition.Eur;

    /// <summary>
    /// Whether to write the subunits as digits: "пет лева и 42 стотинки".
    /// Same as <see cref="AmountInWords.WithSubunitsAsDigits"/>.
    /// </summary>
    public bool SubunitsAsDigits { get; set; }

    /// <summary>
    /// Whether to use the currency abbreviations: "пет лв. и четиридесет и две ст.".
    /// Same as <see cref="AmountInWords.Abbreviated"/>.
    /// </summary>
    public bool Abbreviated { get; set; }

    /// <summary>
    /// Whether to capitalize the first letter: "Пет лева".
    /// Same as <see cref="AmountInWords.Capitalized"/>.
    /// </summary>
    public bool Capitalized { get; set; }
}
