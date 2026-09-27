namespace FluentBgWords;

using FluentBgWords.Internals;

/// <summary>
/// Writes amounts in Bulgarian words with settings configured once, independent of any amount.
/// Produces the same text as the fluent API with the same settings:
/// <c>formatter.Format(amount)</c> equals <c>amount.InWords().As(currency)…ToString()</c>.
/// </summary>
/// <remarks>
/// The settings are copied when the formatter is created, so later changes to the
/// <see cref="AmountWordsOptions"/> object have no effect. A formatter is immutable and
/// thread-safe: create it once and share it, for example by registering it as a singleton.
/// </remarks>
public sealed class AmountWordsFormatter : IAmountWordsFormatter
{
    // Same text as AmountWordsOptionsValidator.NullCurrencyMessage in FluentBgWords.DependencyInjection.
    private const string NullCurrencyMessage =
        $"{nameof(AmountWordsOptions)}.{nameof(AmountWordsOptions.Currency)} must not be null. " +
        $"Set it to a {nameof(CurrencyDefinition)}, such as {nameof(CurrencyDefinition)}.{nameof(CurrencyDefinition.Eur)}.";

    private readonly AmountWordsSettings settings;

    /// <summary>Creates a formatter with the default settings: euro, no other options.</summary>
    public AmountWordsFormatter()
        : this(new AmountWordsOptions())
    {
    }

    /// <summary>Creates a formatter with the given settings.</summary>
    /// <param name="options">The settings. They are copied; later changes to this object have no effect.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="options"/> or its <see cref="AmountWordsOptions.Currency"/> is <see langword="null"/>.
    /// </exception>
    public AmountWordsFormatter(AmountWordsOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.Currency is null)
        {
            throw new ArgumentNullException(
                nameof(options),
                NullCurrencyMessage);
        }

        this.settings = new(
            options.Currency,
            new AmountFormat(
                options.SubunitsAsDigits,
                options.Abbreviated,
                options.Capitalized));
    }

    /// <inheritdoc/>
    public string Format(decimal amount)
        => this.settings.Write(amount);
}
